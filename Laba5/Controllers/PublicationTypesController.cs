using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba4.Models;
using Laba4.ViewModels;
using Laba4.ViewModels.Filters;
using Microsoft.IdentityModel.Tokens;
using Laba4.ViewModels.Sort;

namespace Laba4.Controllers
{
    public class PublicationTypesController : Controller
    {
        private readonly SubsCityContext _context;

        public PublicationTypesController(SubsCityContext context)
        {
            _context = context;
        }

        // GET: PublicationTypes
        public async Task<IActionResult> Index(SortState sortOrder, int page = 1)
        {
            int pageSize = 10;

            var data = _context.PublicationTypes.ToList();

            if (Request.Cookies.TryGetValue("Type", out string type))
            {
                if (!string.IsNullOrEmpty(type))
                {
                    data = data.Where(t => t.Type == type).ToList();
                }
            }


            ViewData["TypeSort"] = sortOrder == SortState.TypeAac ? SortState.TypeDesc : SortState.TypeAac;


            data = sortOrder switch
            {
                SortState.TypeAac => data.OrderByDescending(t => t.Type).ToList(),
                SortState.TypeDesc => data.OrderBy(t => t.Type).ToList()

            };

            var count = data.Count();
            var items = data.Skip((page - 1) * pageSize).Take(pageSize);

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            PublicationTypeIndexViewModel viewModel = new PublicationTypeIndexViewModel(items, pageViewModel)
            {
                PublicationFilter = new PublicationTypeFilterModel()
                {
                    PublicationType = type
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(PublicationTypeFilterModel publicationTypeFilter,int page = 1)
        {
            Response.Cookies.Append("Type", publicationTypeFilter.PublicationType != null ? publicationTypeFilter.PublicationType : "");

            int pageSize = 10;

            var data = _context.PublicationTypes.ToList();

            if (!string.IsNullOrEmpty(publicationTypeFilter.PublicationType))
            {
                data = data.Where(t => t.Type == publicationTypeFilter.PublicationType).ToList();
            }

            var count = data.Count();
            var items = data.Skip((page - 1) * pageSize).Take(pageSize);

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            PublicationTypeIndexViewModel viewModel = new PublicationTypeIndexViewModel(items, pageViewModel)
            {
                PublicationFilter = new PublicationTypeFilterModel() 
                {
                    PublicationType = publicationTypeFilter.PublicationType
                }
            };

            return View(viewModel);
        }

        // GET: PublicationTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PublicationTypes == null)
            {
                return NotFound();
            }

            var publicationType = await _context.PublicationTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publicationType == null)
            {
                return NotFound();
            }

            return View(publicationType);
        }

        // GET: PublicationTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PublicationTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type")] PublicationType publicationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publicationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publicationType);
        }

        // GET: PublicationTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PublicationTypes == null)
            {
                return NotFound();
            }

            var publicationType = await _context.PublicationTypes.FindAsync(id);
            if (publicationType == null)
            {
                return NotFound();
            }
            return View(publicationType);
        }

        // POST: PublicationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type")] PublicationType publicationType)
        {
            if (id != publicationType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publicationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicationTypeExists(publicationType.Id))
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
            return View(publicationType);
        }

        // GET: PublicationTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PublicationTypes == null)
            {
                return NotFound();
            }

            var publicationType = await _context.PublicationTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publicationType == null)
            {
                return NotFound();
            }

            return View(publicationType);
        }

        // POST: PublicationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PublicationTypes == null)
            {
                return Problem("Entity set 'SubsCityContext.PublicationTypes'  is null.");
            }
            var publicationType = await _context.PublicationTypes.FindAsync(id);
            if (publicationType != null)
            {
                _context.PublicationTypes.Remove(publicationType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublicationTypeExists(int id)
        {
          return (_context.PublicationTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
