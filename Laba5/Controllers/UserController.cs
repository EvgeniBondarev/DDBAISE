using Laba4.Data.Cache;
using Laba4.Models;
using Laba4.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostCity.Models;
using PostCity.ViewModels.Filters;
using PostCity.ViewModels;
using Laba4.ViewModels.Filters.FilterModel;
using System.Net;
using PostCity.Data.Cookies;
using PostCity.Infrastructure.Filters;
using PostCity.ViewModels.Sort;
using Laba4.ViewModels.Sort;
using PostCity.Data.Cache;

namespace Laba4.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<PostCityUser> _userManager;
        private readonly UserCache _cache;
        private readonly PostCityContext _context;
        private readonly CookiesManeger _cookies;
        private readonly FilterBy<UserViewModel> _filter;
        private readonly RecipientCache _recipientCache;
        private readonly EmployeeCache _employeeCache;

        public UserController(RoleManager<IdentityRole> roleManager,
                              UserManager<PostCityUser> userManager, 
                              PostCityContext context,
                              UserCache cache,
                              CookiesManeger cookiesManeger,
                              FilterBy<UserViewModel> filter,
                              RecipientCache recipientCache,
                              EmployeeCache employeeCache)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _cache = cache;
            _cookies = cookiesManeger;
            _filter = filter;
            _recipientCache = recipientCache;
            _employeeCache = employeeCache;
        }
        public async Task<IActionResult> Index(UserSortState sortOrder = UserSortState.StandardState, int page = 1)
        {
            var users = _cache.Get();

            UserFilterModel filterData = _cookies.GetFromCookies<UserFilterModel>(Request.Cookies, "UserFilterData");


            SetSortOrderViewData(sortOrder);
            users = ApplySortOrder(users, sortOrder);

            int pageSize = 10;
            _cache.Set(users);

            var pageViewModel = new PageViewModel<UserViewModel, UserFilterModel>(users, page, pageSize, filterData);
            return View(pageViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Index(UserFilterModel filterData, int page = 1)
        {
            _cache.Update();
            _cookies.SaveToCookies(Response.Cookies, "UserFilterData", filterData);

            var users = _cache.Get();

            users = _filter.FilterByString(users, fn => fn.UserObj != null ? fn.UserObj.FullName : "", filterData.Name).ToList();
            users = _filter.FilterByString(users, e => e.Email, filterData.Email).ToList();
            users = _filter.FilterByString(users, r => r.Role, filterData.Role).ToList();

            int pageSize = 10;
            _cache.Set(users);

            var pageViewModel = new PageViewModel<UserViewModel, UserFilterModel>(users, page, pageSize, filterData);
            return View(pageViewModel);

        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    _cache.Update();
                    _employeeCache.Update();
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            PostCityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userRole = roles.FirstOrDefault();

                    if (userRole == "Employee")
                    {
                        var employee = await _context.Employees.FindAsync(user.UserId);
                        if (employee != null)
                        {
                            _context.Employees.Remove(employee);
                            _employeeCache.Update();
                        }
                    }
                    else if(userRole == "Recipient")
                    {
                        var recipient = await _context.Recipients.FindAsync(user.UserId);
                        if (recipient != null)
                        {
                            _context.Recipients.Remove(recipient);
                            _recipientCache.Update();
                        }
                    }

                    int isDelete = await _context.SaveChangesAsync();

                    if(isDelete == 1)
                    {

                        await _userManager.DeleteAsync(user);
                    }
                
                    _cache.Update();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Roles() => View(_roleManager.Roles);

        public void SetSortOrderViewData(UserSortState sortOrder)
        {
            ViewData["RoleSort"] = sortOrder == UserSortState.RoleAsc
                ? UserSortState.RoleDesc
                : UserSortState.RoleAsc;
        }

        public IEnumerable<UserViewModel> ApplySortOrder(IEnumerable<UserViewModel> postCityContext, UserSortState sortOrder)
        {
            return sortOrder switch
            {
                UserSortState.RoleAsc => postCityContext.OrderByDescending(r => r.Role),
                UserSortState.RoleDesc => postCityContext.OrderBy(r => r.Role),

                UserSortState.StandardState => postCityContext,
            };
        }
    }
}
