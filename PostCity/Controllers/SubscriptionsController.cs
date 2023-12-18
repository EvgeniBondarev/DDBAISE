using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Newtonsoft.Json.Linq;
using Domains.Models;
using Domains.ViewModels.Sort;
using Repository;
using Service.Data.Cache;
using Service.Data.Cookies;
using Utils;
using Domains.ViewModels;
using Domains.ViewModels.Filters.FilterModel;

namespace PostCity.Controllers
{
    
    public class SubscriptionsController : Controller, ISortOrderController<Subscription, SubscriptionSortState>
    {
        private readonly PostCityContext _context;
        private readonly SubscriptionCache _cache;
        private readonly CookiesManeger _cookies;
        private readonly FilterBy<Subscription> _filter;
        private readonly SessionLogger _logger;

        public SubscriptionsController(PostCityContext context,
                                       SubscriptionCache cache,
                                       CookiesManeger cookiesManeger,
                                       FilterBy<Subscription> filter,
                                       SessionLogger logger)
        {
            _context = context;
            _cache = cache;
            _cookies = cookiesManeger;
            _filter = filter;
            _logger = logger;
        }

        // GET: Subscriptions
        public IActionResult Index(SubscriptionSortState sortOrder = SubscriptionSortState.StandardState, int page = 1)
        {
            var postCityContext = _cache.Get();


            SubscriptionFilterModel filterData = _cookies.GetFromCookies<SubscriptionFilterModel>(Request.Cookies, "SubscriptionFilterData");

            SetSortOrderViewData(sortOrder);
            postCityContext = ApplySortOrder(postCityContext, sortOrder);

            

            int pageSize = 10;
            _cache.Set(postCityContext);

            var pageViewModel = new PageViewModel<Subscription, SubscriptionFilterModel>(postCityContext, page, pageSize, filterData);
            return View(pageViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(SubscriptionFilterModel filterData, int page = 1)
        {
            _cache.Update();
            _cookies.SaveToCookies(Response.Cookies, "SubscriptionFilterData", filterData);

            IEnumerable<Subscription> data = _cache.Get();

            data = _filter.FilterByInt(data, d => d.Duration, filterData.Duration);
            data = _filter.FilterByDate(data, sb => sb.SubscriptionStartDate, filterData.StartDate);
            data = _filter.FilterByString(data, pn => pn.Office.StreetName, filterData.OfficeName);
            data = _filter.FilterByString(data, pn => pn.Publication.Name, filterData.PublicationName);
            data = _filter.FilterByString(data, pn => pn.Recipient.FullName, filterData.RecipientName);
            data = _filter.FilterByPeriod(data, pn => pn.SubscriptionStartDate, filterData.StartPeriod, filterData.EndPeriod);

            var publicationPriceSum = data.Sum(subscription => subscription.Publication.Price);

            int pageSize = 10;
            _cache.Set(data);

            var pageViewModel = new PageViewModel<Subscription, SubscriptionFilterModel>(data, page, pageSize, filterData);

            pageViewModel.Info = GetInfo(filterData, publicationPriceSum);

            return View(pageViewModel);
        }
        public string GetInfo(SubscriptionFilterModel filterData, decimal publicationPriceSum)
        {
            string info = "Total publication price ";
            if(filterData.StartPeriod != null && filterData.EndPeriod != null)
            {
                info += $"for the period from {filterData.StartPeriod.Value.ToString("dd.MM.yyyy")} to {filterData.EndPeriod.Value.ToString("dd.MM.yyyy")} ";
            }
            else if(filterData.StartPeriod != null && filterData.EndPeriod == null)
            {
                info += $"starting from {filterData.StartPeriod.Value.ToString("dd.MM.yyyy")} ";
            }
            else if (filterData.StartPeriod == null && filterData.EndPeriod != null)
            {
                info += $"ending from {filterData.EndPeriod.Value.ToString("dd.MM.yyyy")} ";
            }
            info += $"is {publicationPriceSum} руб ";
            if(filterData.OfficeName != null)
            {
                info += $"for an office '{filterData.OfficeName}'";
            }
            info += ".";
            return info;
        }
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

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName");
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Name");
            ViewData["RecipientId"] = new SelectList(_context.Recipients, "Id", "FullName");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RecipientId,PublicationId,Duration,OfficeId,SubscriptionStartDate")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscription);
                await _context.SaveChangesAsync();
                _cache.Update();
                _logger.LogInformation($"Add new subscription ({subscription.Publication.Name} / {subscription.Duration} мес.)");
                return RedirectToAction(nameof(Index));
            }
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName", subscription.OfficeId);
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Name", subscription.PublicationId);
            ViewData["RecipientId"] = new SelectList(_context.Recipients, "Id", "FullName", subscription.RecipientId);
            return View(subscription);
        }

        
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
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName", subscription.OfficeId);
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Name", subscription.PublicationId);
            ViewData["RecipientId"] = new SelectList(_context.Recipients, "Id", "FullName", subscription.RecipientId);
            return View(subscription);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecipientId,PublicationId,Duration,OfficeId,SubscriptionStartDate")] Subscription subscription)
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
                    _logger.LogInformation($"Edit subscription ({subscription.Publication.Name} / {subscription.Duration} мес.)");
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
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "StreetName", subscription.OfficeId);
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Name", subscription.PublicationId);
            ViewData["RecipientId"] = new SelectList(_context.Recipients, "Id", "Email", subscription.RecipientId);
            return View(subscription);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
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

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subscriptions == null)
            {
                return Problem("Entity set 'PostCityContext.Subscriptions'  is null.");
            }
            var subscription = await _context.Subscriptions.Include(s => s.Publication).FirstOrDefaultAsync(s => s.Id == id);
            if (subscription != null)
            {
                _logger.LogInformation($"Delete subscription ({subscription.Publication.Name} / {subscription.Duration} мес.)");
                _context.Subscriptions.Remove(subscription);
               
            }

            await _context.SaveChangesAsync();
            _cache.Update();
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
