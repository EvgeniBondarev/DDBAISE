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
        public async Task<IActionResult> Index(int page = 1)
        {
            var data = _cache.Get();

            int durationInt;
            int year;
            decimal price;

            if (Request.Cookies.TryGetValue("Duration", out string durationCookie))
            {
                if (durationCookie != null && Int32.TryParse(durationCookie, out durationInt))
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
            int pageSize = 10;

            var count = data.Count();
            var items = data.Skip((page - 1) * pageSize).Take(pageSize);

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            SubscriptionIndexViewModel viewModel = new SubscriptionIndexViewModel(items, pageViewModel)
            {
                StandardDuration = durationCookie,
                StandardSubscriptionStartDate = subscriptionStartDateCookie,
                StandardPublicationName = subscriptionNameCookie,
                StandardPublicationPrice = subscriptionPriceCookie,
                StandardPublicationType = subscriptionTypeCookie
            };

            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Index(string duration, string subscriptionStartDate,
                                               string publicationName, string publicationPrice, 
                                               string publicationType, int page = 1)
        {

            Response.Cookies.Append("Duration", duration != null ? duration : "");
            Response.Cookies.Append("SubscriptionStartDate", subscriptionStartDate != null ? subscriptionStartDate : "");
            Response.Cookies.Append("SubscriptionName", publicationName != null ? publicationName : "");
            Response.Cookies.Append("SubscriptionPrice", publicationPrice != null ? publicationPrice : "");
            Response.Cookies.Append("SubscriptionType", publicationType != null ? publicationType : "");

            var data = _cache.Get();

            int durationInt;
            int year;
            decimal price;

            if (duration != null && Int32.TryParse(duration, out durationInt))
            {
                data = data.Where(d => d.Duration == durationInt);
            }

            if (int.TryParse(subscriptionStartDate, out year))
            {
                if (year >= 1 && year <= 9999)
                {
                    data = data.Where(sb => DateTime.Parse(sb.SubscriptionStartDate).Year == year);
                }
            }

            if (!string.IsNullOrEmpty(publicationName))
            {
                data = data.Where(pn => pn.Publication.Name == publicationName);
            }

            if (decimal.TryParse(publicationPrice, out price))
            {
                data = data.Where(pp => pp.Publication.Price > price);
            }

            if (!string.IsNullOrEmpty(publicationType))
            {
                data = data.Where(pn => pn.Publication.Type.Type == publicationType);
            }

            int pageSize = 10;

            var count = data.Count();
            var items = data.Skip((page - 1) * pageSize).Take(pageSize);

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            SubscriptionIndexViewModel viewModel = new SubscriptionIndexViewModel(items, pageViewModel)
            {
               StandardDuration = duration,
               StandardSubscriptionStartDate = subscriptionStartDate,
               StandardPublicationName = publicationName,
               StandardPublicationPrice = publicationPrice,
               StandardPublicationType = publicationType
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
