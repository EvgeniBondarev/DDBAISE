using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba4.Models;
using PostCity.Models;
using Microsoft.Data.SqlClient;
using PostCity.ViewModels.Filters.FilterModel;
using PostCity.ViewModels;
using System.Net;
using PostCity.ViewModels.Sort;
using Laba4.ViewModels.Sort;
using PostCity.Data.Cache;
using PostCity.Data.Cookies;
using PostCity.Infrastructure.Filters;
using Laba4.Data.Cache;
using Laba4.ViewModels.Filters.FilterModel;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Laba4.Controllers
{
 
    public class PublicationsController : Controller
    {
        private readonly PostCityContext _context;
        private readonly PublicationCache _cache;
        private readonly CookiesManeger _cookies;
        private readonly FilterBy<Publication> _filter;
        private readonly CacheUpdater _cacheUpdater;
        private readonly SessionLogger _logger;

        public PublicationsController(PostCityContext context,
                                      PublicationCache publicationCache,
                                      CookiesManeger cookies,
                                      FilterBy<Publication> filter,
                                      CacheUpdater cacheUpdater,
                                      SessionLogger sessionLogger)
        { 
            _context = context;
            _cache = publicationCache;
            _cookies = cookies;
            _filter = filter;
            _cacheUpdater = cacheUpdater;
            _logger = sessionLogger;
        }

      
        public async Task<IActionResult> Index(PublicationSortState sortOrder = PublicationSortState.StandardState, int page = 1)
        {
            var postCityContext = _cache.Get();
            PublicationFilterModel filterData = _cookies.GetFromCookies<PublicationFilterModel>(Request.Cookies, "PublicationFilterData");

            SetSortOrderViewData(sortOrder);
            postCityContext = ApplySortOrder(postCityContext, sortOrder);

            int pageSize = 10;
            _cache.Set(postCityContext);

            var pageViewModel = new PageViewModel<Publication, PublicationFilterModel>(postCityContext, page, pageSize, filterData);

            return View(pageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(PublicationFilterModel filterData, int page = 1)
        {
            _cache.Update();
            _cookies.SaveToCookies(Response.Cookies, "PublicationFilterData", filterData);

            var data = _cache.Get();

            data = _filter.FilterByString(data, pn => pn.Name, filterData.Name);
            data = _filter.FilterByString(data, pn => pn.Type.Type, filterData.Type);
            data = _filter.FilterByDecimal(data, pn => pn.Price, filterData.Price);


            int pageSize = 10;
            _cache.Set(data);

            var pageViewModel = new PageViewModel<Publication, PublicationFilterModel>(data, page, pageSize, filterData);
            return View(pageViewModel);
        }

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

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.PublicationTypes, "Id", "Type");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypeId,Name,Price")] Publication publication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publication);
                await _context.SaveChangesAsync();
                _cacheUpdater.Update(_cache);
                _logger.LogInformation($"Add new publication ({publication.Name})");
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.PublicationTypes, "Id", "Type", publication.TypeId);
            return View(publication);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name,Price")] Publication publication)
        {
            if (id != publication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publication);
                    await _context.SaveChangesAsync();
                    _cacheUpdater.Update(_cache);
                    _logger.LogInformation($"Edit publication ({publication.Name})");
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Publications == null)
            {
                return Problem("Entity set 'PostCityContext.Publications'  is null.");
            }
            var publication = await _context.Publications.FindAsync(id);
            if (publication != null)
            {
                _context.Publications.Remove(publication);
                _logger.LogInformation($"Delete publication ({publication.Name})");
            }
            
            await _context.SaveChangesAsync();
            _cacheUpdater.Update(_cache);
            return RedirectToAction(nameof(Index));
        }

        private bool PublicationExists(int id)
        {
          return (_context.Publications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public void SetSortOrderViewData(PublicationSortState sortOrder)
        {
            ViewData["NameSort"] = sortOrder == PublicationSortState.NameAsc
                ? PublicationSortState.NameDesc
                : PublicationSortState.NameAsc;

            ViewData["TypeSort"] = sortOrder == PublicationSortState.TypeAsc
                ? PublicationSortState.TypeDesc
                : PublicationSortState.TypeAsc;

            ViewData["PriceSort"] = sortOrder == PublicationSortState.PriceAsc
                ? PublicationSortState.PriceDesc
                : PublicationSortState.PriceAsc;
        }

        public IEnumerable<Publication> ApplySortOrder(IEnumerable<Publication> postCityContext, PublicationSortState sortOrder)
        {
            return sortOrder switch
            {
                PublicationSortState.NameDesc => postCityContext.OrderByDescending(n => n.Name),
                PublicationSortState.NameAsc => postCityContext.OrderBy(n => n.Name),

                PublicationSortState.TypeDesc => postCityContext.OrderByDescending(t => t.Type.Type),
                PublicationSortState.TypeAsc => postCityContext.OrderBy(t => t.Type.Type),

                PublicationSortState.PriceDesc => postCityContext.OrderByDescending(p => p.Price),
                PublicationSortState.PriceAsc => postCityContext.OrderBy(p => p.Price),

                PublicationSortState.StandardState => postCityContext
            };
        }
    }
}
