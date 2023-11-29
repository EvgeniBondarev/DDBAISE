using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Laba4.Data;
using Laba4.Data.Cache;
using Laba4.Models;
using Laba4.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PostCity.Data;
using PostCity.Data.Cache;
using PostCity.Data.Cookies;
using PostCity.Infrastructure.Filters;
using PostCity.Models;
using PostCity.ViewModels;
using PostCity.ViewModels.Filters;
using PostCity.ViewModels.Filters.FilterModel;
using PostCity.ViewModels.Sort;

namespace PostCity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeesController : Controller
    {
        private readonly PostCityContext _context;
        private readonly EmployeeCache _cache;
        private readonly CookiesManeger _cookies;
        private readonly FilterBy<Employee> _filter;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly UserManager<PostCityUser> _userManager;
        private readonly CacheUpdater _cacheUpdater;
        private readonly SessionLogger _logger;

        public EmployeesController(PostCityContext context,
                                   EmployeeCache employeeCache,
                                   CookiesManeger cookiesManeger,
                                   FilterBy<Employee> filter,
                                   UserRegistrationManager userRegistrationManager,
                                   UserManager<PostCityUser> userManager,
                                   CacheUpdater cacheUpdater,
                                   SessionLogger logger)
        {
            _context = context;
            _cache = employeeCache;
            _cookies = cookiesManeger;
            _filter = filter;
            _userRegistrationManager = userRegistrationManager;
            _userManager = userManager;
            _cacheUpdater = cacheUpdater;
            _logger = logger;
        }

        // GET: Employees
        public async Task<IActionResult> Index(EmployeeSortState sortOrder = EmployeeSortState.StandardState, int page = 1)
        {
            var postCityContext = _cache.Get();

            EmployeeFilterModel filterData = _cookies.GetFromCookies<EmployeeFilterModel>(Request.Cookies, "EmployeeFilterData");

            SetSortOrderViewData(sortOrder);
            postCityContext = ApplySortOrder(postCityContext, sortOrder).ToList();

            int pageSize = 15;

            var pageViewModel = new PageViewModel<Employee, EmployeeFilterModel>(postCityContext, page, pageSize, filterData);
            return View(pageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(EmployeeFilterModel filterData, int page = 1)
        {
            _cache.Update();
            _cookies.SaveToCookies(Response.Cookies, "EmployeeFilterData", filterData);

            var data = _cache.Get();


            data = _filter.FilterByString(data, pn => pn.Name, filterData.Name);
            data = _filter.FilterByString(data, pn => pn.Middlename, filterData.Middlename);
            data = _filter.FilterByString(data, pn => pn.Surname, filterData.Surname);
            data = _filter.FilterByString(data, pn => pn.Office.StreetName, filterData.Office);
            data = _filter.FilterByString(data, pn => pn.Position.Position, filterData.Position);

            int pageSize = 15;
            _cache.Set(data);
            var pageViewModel = new PageViewModel<Employee, EmployeeFilterModel>(data, page, pageSize, filterData);
            return View(pageViewModel);
        }
        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = _cache.Get().FirstOrDefault(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
       [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName");
            ViewData["PositionId"] = new SelectList(_context.EmployeePositions, "Id", "Position");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Middlename,Surname,PositionId,OfficeId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (Request.Cookies.TryGetValue("EmployeeCredentials", out string credentialsJson))
                {
                    var credentials = JsonConvert.DeserializeAnonymousType(credentialsJson, new { Email = "", Password = "" });

                    using var transaction = await _context.Database.BeginTransactionAsync();

                    try
                    {

                        _context.Add(employee);
                        await _context.SaveChangesAsync();

                        var result = await _userRegistrationManager.RegisterUserWithRole(new PostCityUserModel()
                        {
                            Role = "Employee",
                            Email = credentials.Email,
                            Password = credentials.Password,
                            UserId = employee.Id
                        });

                        if (result.Succeeded)
                        {
                            transaction.Commit();
                            _cacheUpdater.Update(_cache);
                            _logger.LogInformation($"Add new employee ({employee.FullName})");
                            return Redirect("/Role/UserList");

                        }
                        else
                        {
                            transaction.Rollback();
                            _context.Employees.Remove(employee);

                            foreach (var error in result.Errors)
                            {
                                ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName");
                                ViewData["PositionId"] = new SelectList(_context.EmployeePositions, "Id", "Position");
                                return View(employee);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName");
            ViewData["PositionId"] = new SelectList(_context.EmployeePositions, "Id", "Position");
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName", employee.OfficeId);
            ViewData["PositionId"] = new SelectList(_context.EmployeePositions, "Id", "Position", employee.PositionId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Middlename,Surname,PositionId,OfficeId")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    _cacheUpdater.Update(_cache);
                    _logger.LogInformation($"Edit employee ({employee.FullName})");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName", employee.OfficeId);
            ViewData["PositionId"] = new SelectList(_context.EmployeePositions, "Id", "Position", employee.PositionId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee =_cache.Get().FirstOrDefault(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'PostCityContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _logger.LogInformation($"Delete employee ({employee.FullName})");
            }

            int isDelete = await _context.SaveChangesAsync();
            if (isDelete == 1)
            {
                PostCityUser user = _context.FindUserByUserId(employee.Id);
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            _cacheUpdater.Update(_cache);
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public void SetSortOrderViewData(EmployeeSortState sortOrder)
        {
            ViewData["NameSort"] = sortOrder == EmployeeSortState.NameAsc
                ? EmployeeSortState.NameDesc
                : EmployeeSortState.NameAsc;

            ViewData["MiddlenameSort"] = sortOrder == EmployeeSortState.MiddlenameAsc
                ? EmployeeSortState.MiddlenameDesc
                : EmployeeSortState.MiddlenameAsc;

            ViewData["SurnameSort"] = sortOrder == EmployeeSortState.SurnameAsc
                ? EmployeeSortState.SurnameDesc
                : EmployeeSortState.SurnameAsc;

            ViewData["OfficeSort"] = sortOrder == EmployeeSortState.OfficeAsc
                ? EmployeeSortState.OfficeDesc
                : EmployeeSortState.OfficeAsc;

            ViewData["PositionSort"] = sortOrder == EmployeeSortState.PositionAsc
                ? EmployeeSortState.PositionDesc
                : EmployeeSortState.PositionAsc;
        }

        public IEnumerable<Employee> ApplySortOrder(IEnumerable<Employee> postCityContext, EmployeeSortState sortOrder)
        {
            return sortOrder switch
            {
                EmployeeSortState.NameDesc => postCityContext.OrderByDescending(n => n.Name),
                EmployeeSortState.NameAsc => postCityContext.OrderBy(n => n.Name),

                EmployeeSortState.MiddlenameDesc => postCityContext.OrderByDescending(m => m.Middlename),
                EmployeeSortState.MiddlenameAsc => postCityContext.OrderBy(m => m.Middlename),

                EmployeeSortState.SurnameDesc => postCityContext.OrderByDescending(s => s.Surname),
                EmployeeSortState.SurnameAsc => postCityContext.OrderBy(s => s.Surname),

                EmployeeSortState.OfficeDesc => postCityContext.OrderByDescending(o => o.Office.StreetName),
                EmployeeSortState.OfficeAsc => postCityContext.OrderBy(o => o.Office.StreetName),

                EmployeeSortState.PositionDesc => postCityContext.OrderByDescending(p => p.Position.Position),
                EmployeeSortState.PositionAsc => postCityContext.OrderBy(p => p.Position.Position),

                EmployeeSortState.StandardState => postCityContext.ToList()
            };
        }
    }
}
