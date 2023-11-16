using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba4.Models;
using Laba4.Data.Cache;
using Laba4.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Http;
using System.Data.SqlTypes;
using Laba4.ViewModels.Filters;
using Laba4.ViewModels.Sort;
using Microsoft.Data.SqlClient;

namespace Laba4.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly SubsCityContext _context;
        private readonly SubscriptionCache _cache;
        public SubscriptionsController(SubsCityContext context, SubscriptionCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Subscriptions
        public async Task<IActionResult> Index(SortState sortOrder, int page = 1)
        {
            var data = _cache.Get();

            int durationInt;
            int year;
            decimal price;

            if (Request.Cookies.TryGetValue("Duration", out string subscriptionDurationCookie))
            {
                if (subscriptionDurationCookie != null && Int32.TryParse(subscriptionDurationCookie, out durationInt))
                {
                    data = data.Where(d => d.Duration == durationInt);
                }
            }

            if (Request.Cookies.TryGetValue("SubscriptionStartDate", out string subscriptionStartDateCookie))
            {
                if(int.TryParse(subscriptionStartDateCookie, out year))
                {
                        if (year >= 1 && year <= 9999)
                        {
                            data = data.Where(sb => DateTime.Parse(sb.SubscriptionStartDate).Year == year);
                        }
                    }
                }

            if (Request.Cookies.TryGetValue("SubscriptionName", out string subscriptionNameCookie))
            {
                if (!string.IsNullOrEmpty(subscriptionNameCookie))
                {
                    data = data.Where(pn => pn.Publication.Name == subscriptionNameCookie);
                }
            }

            if (Request.Cookies.TryGetValue("SubscriptionPrice", out string subscriptionPriceCookie))
            {
                if (decimal.TryParse(subscriptionPriceCookie, out price))
                {
                    data = data.Where(pp => pp.Publication.Price > price);
                }
            }

            if (Request.Cookies.TryGetValue("SubscriptionType", out string subscriptionTypeCookie))
            {
                if (!string.IsNullOrEmpty(subscriptionTypeCookie))
                {
                    data = data.Where(pn => pn.Publication.Type.Type == subscriptionTypeCookie);
                }
            }

            ViewData["PriceSort"] = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["DurationSort"] = sortOrder == SortState.DurationAsc ? SortState.PriceDesc : SortState.DurationAsc;
            ViewData["TypeSort"] = sortOrder == SortState.TypeAac ? SortState.TypeDesc : SortState.TypeAac;

            data = sortOrder switch
            {
                SortState.PriceDesc => data.OrderByDescending(p => p.Publication.Price),
                SortState.PriceAsc => data.OrderBy(p => p.Publication.Price),

                SortState.NameAsc => data.OrderByDescending(n => n.Publication.Name),
                SortState.NameDesc => data.OrderBy(n => n.Publication.Name),

                SortState.DurationDesc => data.OrderByDescending(d => d.Duration),
                SortState.DurationAsc => data.OrderBy(d => d.Duration),

                SortState.TypeAac => data.OrderByDescending(t => t.Publication.Type.Type),
                SortState.TypeDesc => data.OrderBy(t => t.Publication.Type.Type)
            };

            int pageSize = 10;

            var count = data.Count();
            var items = data.Skip((page - 1) * pageSize).Take(pageSize);

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            SubscriptionIndexViewModel viewModel = new SubscriptionIndexViewModel(items, pageViewModel)
            {
                SubscriptionFilter  = new SubscriptionFilterModel()
                {

                    Duration = subscriptionDurationCookie,
                    StartDate = subscriptionStartDateCookie,
                    PublicationName = subscriptionNameCookie,
                    PublicationPrice = subscriptionPriceCookie,
                    PublicationType = subscriptionTypeCookie
                }
            };

            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Index(SubscriptionFilterModel filterData, int page = 1)     
        {

            Response.Cookies.Append("Duration", filterData.Duration != null ? filterData.Duration : "");
            Response.Cookies.Append("SubscriptionStartDate", filterData.StartDate != null ? filterData.StartDate : "");
            Response.Cookies.Append("SubscriptionName", filterData.PublicationName != null ? filterData.PublicationName : "");
            Response.Cookies.Append("SubscriptionPrice", filterData.PublicationPrice != null ? filterData.PublicationPrice : "");
            Response.Cookies.Append("SubscriptionType", filterData.PublicationType != null ? filterData.PublicationType : "");

            var data = _cache.Get();

            int durationInt;
            int year;
            decimal price;

            if (filterData.Duration != null && Int32.TryParse(filterData.Duration, out durationInt))
            {
                data = data.Where(d => d.Duration == durationInt);
            }

            if (int.TryParse(filterData.StartDate, out year))
            {
                if (year >= 1 && year <= 9999)
                {
                    data = data.Where(sb => DateTime.Parse(sb.SubscriptionStartDate).Year == year);
                }
            }

            if (!string.IsNullOrEmpty(filterData.PublicationName))
            {
                data = data.Where(pn => pn.Publication.Name == filterData.PublicationName);
            }

            if (decimal.TryParse(filterData.PublicationPrice, out price))
            {
                data = data.Where(pp => pp.Publication.Price > price);
            }

            if (!string.IsNullOrEmpty(filterData.PublicationType))
            {
                data = data.Where(pn => pn.Publication.Type.Type == filterData.PublicationType);
            }

            int pageSize = 10;

            var count = data.Count();
            var items = data.Skip((page - 1) * pageSize).Take(pageSize);



            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            SubscriptionIndexViewModel viewModel = new SubscriptionIndexViewModel(items, pageViewModel)
            {
                SubscriptionFilter = filterData
            };

            return View(viewModel);

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
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Name");
            return View();
        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PublicationId,Duration,SubscriptionStartDate")] Subscription subscription)
        {
            if (ModelState.ErrorCount < 2)
            {
                _context.Add(subscription);
                await _context.SaveChangesAsync();
                _cache.Set();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Name", subscription.PublicationId);
            return View(subscription);
        }

        // GET: Subscriptions/Edit/5
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
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Name", subscription.PublicationId);
            return View(subscription);
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PublicationId,Duration,SubscriptionStartDate")] Subscription subscription)
        {
            if (id != subscription.Id)
            {
                return NotFound();
            }

            if (ModelState.ErrorCount < 2)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                    _cache.Set();
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
            ViewData["PublicationId"] = new SelectList(_context.Publications, "Id", "Id", subscription.PublicationId);
            return View(subscription);
        }

        // GET: Subscriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.Publication)
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
                return Problem("Entity set 'SubsCityContext.Subscriptions'  is null.");
            }
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
            }
            
            await _context.SaveChangesAsync();
            _cache.Set();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionExists(int id)
        {
          return (_context.Subscriptions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
