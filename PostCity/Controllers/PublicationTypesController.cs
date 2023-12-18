using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Repository;
using Service.Data.Cache;
using Utils;
using Domains.Models;

namespace PostCity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PublicationTypesController : Controller
    {
        private readonly PostCityContext _context;
        private readonly PublicationCache _publicationCache;
        private readonly SessionLogger _logger;

        public PublicationTypesController(PostCityContext context, PublicationCache publicationCache, SessionLogger logger)
        {
            _context = context;
            _publicationCache = publicationCache;
            _logger = logger;
        }

        // GET: PublicationTypes
        public async Task<IActionResult> Index()
        {
              return _context.PublicationTypes != null ? 
                          View(await _context.PublicationTypes.ToListAsync()) :
                          Problem("Entity set 'PostCityContext.PublicationTypes'  is null.");
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
                _logger.LogInformation($"Add new publication type ({publicationType.Type})");
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
                    _publicationCache.Update();
                    _logger.LogInformation($"Edit publication type ({publicationType.Type})");
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
                return Problem("Entity set 'PostCityContext.PublicationTypes'  is null.");
            }
            var publicationType = await _context.PublicationTypes.FindAsync(id);
            if (publicationType != null)
            {
                _context.PublicationTypes.Remove(publicationType);
                _publicationCache.Update();
                _logger.LogInformation($"Delete publication type ({publicationType.Type})");
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
