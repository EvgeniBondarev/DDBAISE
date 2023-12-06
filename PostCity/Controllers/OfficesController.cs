using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PostCity.Data;
using PostCity.Models;
using PostCity.ViewModels.Filters.FilterModel;
using PostCity.ViewModels.Sort;
using PostCity.ViewModels;
using PostCity.Data.Cache;
using PostCity.Data.Cookies;
using PostCity.Infrastructure.Filters;
using PostCity.ViewModels.Filters;
using Laba4.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Laba4.Data.Cache;
using Laba4.Controllers;

namespace PostCity.Controllers
{

    public class OfficesController : Controller, ISortOrderController<Office, OfficeSortState>
    {
        private readonly PostCityContext _context;
        private readonly OfficeCache _cache;
        private readonly CookiesManeger _cookies;
        private readonly FilterBy<Office> _filter;
        private readonly CacheUpdater _cacheUpdater;
        private readonly SessionLogger _logger;

        public OfficesController(PostCityContext context,
                                 OfficeCache officeCache,
                                 CookiesManeger cookiesManeger,
                                 FilterBy<Office> filterBy,
                                 CacheUpdater cacheUpdater,
                                 SessionLogger logger)
        {
            _context = context;
            _cache = officeCache;
            _cookies = cookiesManeger;
            _filter = filterBy;
            _cacheUpdater = cacheUpdater;
            _logger = logger;
        }

        // GET: Offices
        public async Task<IActionResult> Index(OfficeSortState sortOrder = OfficeSortState.StandardState, int page = 1)
        {
            var postCityContext = _cache.Get();

            OfficeFilterModel filterData = _cookies.GetFromCookies<OfficeFilterModel>(Request.Cookies, "OfficeFilterData");

            SetSortOrderViewData(sortOrder);
            postCityContext = ApplySortOrder(postCityContext, sortOrder);

            int pageSize = 10;
            _cache.Set(postCityContext);

            var pageViewModel = new PageViewModel<Office, OfficeFilterModel>(postCityContext, page, pageSize, filterData);

            return View(pageViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(OfficeFilterModel filterData, int page = 1)
        {
            _cache.Update();
            _cookies.SaveToCookies(Response.Cookies, "OfficeFilterData", filterData);

            var data = _cache.Get();

            data = _filter.FilterByString(data, pn => pn.OwnerName, filterData.OwnerName);
            data = _filter.FilterByString(data, pn => pn.OwnerSurname, filterData.OwnerSurname);
            data = _filter.FilterByString(data, pn => pn.OwnerMiddlename, filterData.OwnerMiddlename);
            data = _filter.FilterByString(data, pn => pn.StreetName, filterData.StreetName);
            data = _filter.FilterByString(data, pn => pn.MobilePhone, filterData.MobilePhone);
            data = _filter.FilterByString(data, pn => pn.Email, filterData.Email);

            int pageSize = 10;
            _cache.Set(data);   

            var pageViewModel = new PageViewModel<Office, OfficeFilterModel>(data, page, pageSize, filterData);
            return View(pageViewModel);
        }

        // GET: Offices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Offices == null)
            {
                return NotFound();
            }

            var office = _cache.Get().FirstOrDefault(m => m.Id == id);
            if (office == null)
            {
                return NotFound();
            }

            return View(office);
        }

        // GET: Offices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Offices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,OwnerName,OwnerMiddlename,OwnerSurname,StreetName,MobilePhone,Email")] Office office)
        {
            if (ModelState.IsValid)
            {
                _context.Add(office);
                await _context.SaveChangesAsync();
                _cacheUpdater.Update(_cache);
                _logger.LogInformation($"Add new office ({office.StreetName})");
                return RedirectToAction(nameof(Index));
            }
            return View(office);
        }

        // GET: Offices/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Offices == null)
            {
                return NotFound();
            }

            var office = await _context.Offices.FindAsync(id);
            if (office == null)
            {
                return NotFound();
            }
            return View(office);
        }

        // POST: Offices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerName,OwnerMiddlename,OwnerSurname,StreetName,MobilePhone,Email")] Office office)
        {
            if (id != office.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(office);
                    await _context.SaveChangesAsync();
                    _cacheUpdater.Update(_cache);
                    _logger.LogInformation($"Edit office ({office.StreetName})");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfficeExists(office.Id))
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
            return View(office);
        }

        // GET: Offices/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Offices == null)
            {
                return NotFound();
            }

            var office = _cache.Get().FirstOrDefault(m => m.Id == id);
            if (office == null)
            {
                return NotFound();
            }

            return View(office);
        }

        // POST: Offices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Offices == null)
            {
                return Problem("Entity set 'PostCityContext.Offices'  is null.");
            }
            var office = await _context.Offices.FindAsync(id);
            if (office != null)
            {
                _context.Offices.Remove(office);
                _logger.LogInformation($"Delete office ({office.StreetName})");
            }
            
            await _context.SaveChangesAsync();
            _cacheUpdater.Update(_cache);
            return RedirectToAction(nameof(Index));
        }

        private bool OfficeExists(int id)
        {
          return (_context.Offices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public void SetSortOrderViewData(OfficeSortState sortOrder)
        {
            ViewData["OwnerNameSort"] = sortOrder == OfficeSortState.OwnerNameAsc
                ? OfficeSortState.OwnerNameDesc
                : OfficeSortState.OwnerNameAsc;

            ViewData["OwnerMiddlenameSort"] = sortOrder == OfficeSortState.OwnerMiddlenameAsc
                ? OfficeSortState.OwnerMiddlenameDecs
                : OfficeSortState.OwnerMiddlenameAsc;

            ViewData["OwnerSurnameSort"] = sortOrder == OfficeSortState.OwnerSurnameAsc
                ? OfficeSortState.OwnerSurnameDesc
                : OfficeSortState.OwnerSurnameAsc;

            ViewData["StreetNameSort"] = sortOrder == OfficeSortState.StreetNameAsc
                ? OfficeSortState.StreetNameDesc
                : OfficeSortState.StreetNameAsc;

        }

        public IEnumerable<Office> ApplySortOrder(IEnumerable<Office> postCityContext, OfficeSortState sortOrder)
        {
            return sortOrder switch
            {
                OfficeSortState.OwnerNameDesc => postCityContext.OrderByDescending(n => n.OwnerName),
                OfficeSortState.OwnerNameAsc => postCityContext.OrderBy(n => n.OwnerName),

                OfficeSortState.OwnerSurnameAsc => postCityContext.OrderByDescending(s => s.OwnerSurname),
                OfficeSortState.OwnerSurnameDesc => postCityContext.OrderBy(s => s.OwnerSurname),

                OfficeSortState.OwnerMiddlenameAsc => postCityContext.OrderByDescending(m => m.OwnerMiddlename),
                OfficeSortState.OwnerMiddlenameDecs => postCityContext.OrderBy(m => m.OwnerMiddlename),

                OfficeSortState.StreetNameAsc => postCityContext.OrderByDescending(s => s.StreetName),
                OfficeSortState.StreetNameDesc => postCityContext.OrderBy(s => s.StreetName),

                OfficeSortState.StandardState => postCityContext.ToList()
            };
        }
    }
}
