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

namespace Laba4.Controllers
{
    public class PublicationsController : Controller
    {
        private readonly SubsCityContext _context;
        private readonly PublicationCache _cache;
        public PublicationsController(SubsCityContext context, PublicationCache publicationCache)
        {
            _context = context;
            _cache = publicationCache;
        }

        // GET: Publications
        public async Task<IActionResult> Index(int page = 1)
        {
            var data = _cache.Get();

            decimal price;

            if (Request.Cookies.TryGetValue("PublicationPrice", out string publicationPriceCookie))
            {
                if (decimal.TryParse(publicationPriceCookie, out price))
                {
                    data = data.Where(p => p.Price > price);
                }
            }

            if (Request.Cookies.TryGetValue("PublicationType", out string publicationTypeCookie))
            {
                if (!string.IsNullOrEmpty(publicationTypeCookie))
                {
                    data = data.Where(t => t.Type.Type == publicationTypeCookie);
                }
            }
            int pageSize = 10;

            var count = data.Count();
            var items = data.Skip((page - 1) * pageSize).Take(pageSize);

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            PublicationIndexViewModel viewModel = new PublicationIndexViewModel(items, pageViewModel)
            {
                StandardPublicationPrice = publicationPriceCookie,
                StandardPublicationType = publicationTypeCookie
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string publicationPrice, string publicationType, int page = 1)
        {

            Response.Cookies.Append("PublicationPrice", publicationPrice != null ? publicationPrice : "");
            Response.Cookies.Append("PublicationType", publicationType != null ? publicationType : "");

            var data = _cache.Get();

            decimal price;

            if (decimal.TryParse(publicationPrice, out price))
            {
                data = data.Where(p => p.Price > price);
            }

            if (!string.IsNullOrEmpty(publicationType))
            {
                data = data.Where(t => t.Type.Type == publicationType);
            }

            int pageSize = 10;

            var count = data.Count();
            var items = data.Skip((page - 1) * pageSize).Take(pageSize);

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            PublicationIndexViewModel viewModel = new PublicationIndexViewModel(items, pageViewModel)
            {
                StandardPublicationPrice = publicationPrice,
                StandardPublicationType = publicationType
            };

            return View(viewModel);

        }

        // GET: Publications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Publications == null)
            {
                return NotFound();
            }

            var publication = _cache.Get().FirstOrDefault(m => m.Id == id);
            if (publication == null)
            {
                return NotFound();
            }

            return View(publication);
        }

        // GET: Publications/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.PublicationTypes, "Id", "Type");
            return View();
        }

        // POST: Publications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,Name,Price")] Publication publication)
        {
            if (ModelState.ErrorCount < 2)
            {
                _context.Add(publication);
                await _context.SaveChangesAsync();
                _cache.Set();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.PublicationTypes, "Id", "Type", publication.TypeId);
            return View(publication);
        }

        // GET: Publications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Publications == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications.FindAsync(id);
            if (publication == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.PublicationTypes, "Id", "Type", publication.TypeId);
            return View(publication);
        }

        // POST: Publications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeId,Name,Price")] Publication publication)
        {
            if (id != publication.Id)
            {
                return NotFound();
            }

            if (ModelState.ErrorCount < 2)
            {
                try
                {
                    _context.Update(publication);
                    await _context.SaveChangesAsync();
                    _cache.Set();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicationExists(publication.Id))
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
            ViewData["TypeId"] = new SelectList(_context.PublicationTypes, "Id", "Type", publication.TypeId);
            return View(publication);
        }

        // GET: Publications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Publications == null)
            {
                return NotFound();
            }

            var publication = _cache.Get().FirstOrDefault(m => m.Id == id);
            if (publication == null)
            {
                return NotFound();
            }

            return View(publication);
        }

        // POST: Publications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Publications == null)
            {
                return Problem("Entity set 'SubsCityContext.Publications'  is null.");
            }
            var publication = await _context.Publications.FindAsync(id);
            if (publication != null)
            {
                _context.Publications.Remove(publication);
            }
            
            await _context.SaveChangesAsync();
            _cache.Set();
            return RedirectToAction(nameof(Index));
        }

        private bool PublicationExists(int id)
        {
          return (_context.Publications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
