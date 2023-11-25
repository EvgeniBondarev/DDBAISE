using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PostCity.Data;
using PostCity.Models;
using PostCity.Data.Cache;
using PostCity.ViewModels.Filters;
using PostCity.ViewModels;
using PostCity.ViewModels.Sort;
using PostCity.Infrastructure.Filters;
using Newtonsoft.Json;
using PostCity.Data.Cookies;
using Laba4.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace PostCity.Controllers
{

    public class SubscriptionsController : Controller
    {
        private readonly PostCityContext _context;
        private readonly SubscriptionCache _cache;
        private readonly CookiesManeger _cookies;
        private readonly FilterBy<Subscription> _filter;

        public SubscriptionsController(PostCityContext context,
                                       SubscriptionCache cache,
                                       CookiesManeger cookiesManeger,
                                       FilterBy<Subscription> filter)
        {
            _context = context;
            _cache = cache;
            _cookies = cookiesManeger;
            _filter = filter;
        }

        // GET: Subscriptions
        public IActionResult Index(SubscriptionSortState sortOrder = SubscriptionSortState.StandardState, int page = 1)
        {
            var postCityContext = _cache.Get();


            SubscriptionFilterModel filterData = _cookies.GetFromCookies<SubscriptionFilterModel>(Request.Cookies, "SubscriptionFilterData");

            SetSortOrderViewData(sortOrder);
            postCityContext = ApplySortOrder(postCityContext, sortOrder);

            int pageSize = 15;
            _cache.Set(postCityContext);

            var pageViewModel = new PageViewModel<Subscription, SubscriptionFilterModel>(postCityContext, page, pageSize, filterData);
            return View(pageViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(SubscriptionFilterModel filterData, int page = 1)
        {
            _cache.Update();
            _cookies.SaveToCookies(Response.Cookies, "SubscriptionFilterData", filterData);

            var data = _cache.Get();

            data = _filter.FilterByInt(data, d => d.Duration, filterData.Duration);
            data = _filter.FilterByDate(data, sb => sb.SubscriptionStartDate, filterData.StartDate);
            data = _filter.FilterByString(data, pn => pn.Office.StreetName, filterData.OfficeName);
            data = _filter.FilterByString(data, pn => pn.Publication.Name, filterData.PublicationName);
            data = _filter.FilterByString(data, pn => pn.Employee.Name, filterData.EmployeeName);

            int pageSize = 15;
            _cache.Set(data);

            var pageViewModel = new PageViewModel<Subscription, SubscriptionFilterModel>(data, page, pageSize, filterData);
            return View(pageViewModel);
        }
        // GET: Subscriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = _cache.Get().FirstOrDefault(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // GET: Subscriptions/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName");
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Name");
            ViewData["RecipientId"] = new SelectList(_context.Recipients, "Id", "FullName");
            return View();
        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(DatabaseSaveFilter))]
        public async Task<IActionResult> Create([Bind("Id,RecipientId,PublicationId,Duration,OfficeId,EmployeeId,SubscriptionStartDate")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscription);
                await _context.SaveChangesAsync();
                _cache.Update();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", subscription.EmployeeId);
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName", subscription.OfficeId);
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Name", subscription.PublicationId);
            ViewData["RecipientId"] = new SelectList(_context.Recipients, "Id", "FullName", subscription.RecipientId);
            return View(subscription);
        }

        // GET: Subscriptions/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", subscription.EmployeeId);
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName", subscription.OfficeId);
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Name", subscription.PublicationId);
            ViewData["RecipientId"] = new SelectList(_context.Recipients, "Id", "FullName", subscription.RecipientId);
            return View(subscription);
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(DatabaseSaveFilter))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecipientId,PublicationId,Duration,OfficeId,EmployeeId,SubscriptionStartDate")] Subscription subscription)
        {
            if (id != subscription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                    _cache.Update();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(subscription.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", subscription.EmployeeId);
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName", subscription.OfficeId);
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Name", subscription.PublicationId);
            ViewData["RecipientId"] = new SelectList(_context.Recipients, "Id", "Email", subscription.RecipientId);
            return View(subscription);
        }

        // GET: Subscriptions/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.Employee)
                .Include(s => s.Office)
                .Include(s => s.Publication)
                .Include(s => s.Recipient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // POST: Subscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subscriptions == null)
            {
                return Problem("Entity set 'PostCityContext.Subscriptions'  is null.");
            }
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionExists(int id)
        {
            return (_context.Subscriptions?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public void SetSortOrderViewData(SubscriptionSortState sortOrder)
        {
            ViewData["DurationSort"] = sortOrder == SubscriptionSortState.DurationAsc
                ? SubscriptionSortState.DurationDesc
                : SubscriptionSortState.DurationAsc;

            ViewData["DateSort"] = sortOrder == SubscriptionSortState.DateAsc
                ? SubscriptionSortState.DateDesc
                : SubscriptionSortState.DateAsc;

            ViewData["OfficeSort"] = sortOrder == SubscriptionSortState.OfficeNameAsc
                ? SubscriptionSortState.OfficeNameDesc
                : SubscriptionSortState.OfficeNameAsc;

            ViewData["PublicationSort"] = sortOrder == SubscriptionSortState.PublicationNameAsc
                ? SubscriptionSortState.PublicationNameDesc
                : SubscriptionSortState.PublicationNameAsc;
        }

        public IEnumerable<Subscription> ApplySortOrder(IEnumerable<Subscription> postCityContext, SubscriptionSortState sortOrder)
        {
            return sortOrder switch
            {
                SubscriptionSortState.DurationDesc => postCityContext.OrderByDescending(d => d.Duration),
                SubscriptionSortState.DurationAsc => postCityContext.OrderBy(d => d.Duration),

                SubscriptionSortState.DateDesc => postCityContext.OrderByDescending(d => d.SubscriptionStartDate),
                SubscriptionSortState.DateAsc => postCityContext.OrderBy(d => d.SubscriptionStartDate),

                SubscriptionSortState.OfficeNameDesc => postCityContext.OrderByDescending(o => o.Office.StreetName),
                SubscriptionSortState.OfficeNameAsc => postCityContext.OrderBy(o => o.Office.StreetName),

                SubscriptionSortState.PublicationNameDesc => postCityContext.OrderByDescending(n => n.Publication.Name),
                SubscriptionSortState.PublicationNameAsc => postCityContext.OrderBy(n => n.Publication.Name),

                SubscriptionSortState.StandardState => postCityContext
            };
        }

    }

}
