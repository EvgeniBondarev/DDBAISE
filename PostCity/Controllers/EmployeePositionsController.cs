using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Repository.Models;
using Utils;
using Domains.Models;

namespace PostCity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeePositionsController : Controller
    {
        private readonly PostCityContext _context;
        private readonly SessionLogger _logger;

        public EmployeePositionsController(PostCityContext context, SessionLogger logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: EmployeePositions
        public async Task<IActionResult> Index()
        {
              return _context.EmployeePositions != null ? 
                          View(await _context.EmployeePositions.ToListAsync()) :
                          Problem("Entity set 'PostCityContext.EmployeePositions'  is null.");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployeePositions == null)
            {
                return NotFound();
            }

            var employeePosition = await _context.EmployeePositions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeePosition == null)
            {
                return NotFound();
            }

            return View(employeePosition);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Position")] EmployeePosition employeePosition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeePosition);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Add new employee position ({employeePosition.Position})");
                return RedirectToAction(nameof(Index));
            }
            return View(employeePosition);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmployeePositions == null)
            {
                return NotFound();
            }

            var employeePosition = await _context.EmployeePositions.FindAsync(id);
            if (employeePosition == null)
            {
                return NotFound();
            }
            return View(employeePosition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Position")] EmployeePosition employeePosition)
        {
            if (id != employeePosition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeePosition);
                    _logger.LogInformation($"Edit employee position ({employeePosition.Position})");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeePositionExists(employeePosition.Id))
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
            return View(employeePosition);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployeePositions == null)
            {
                return NotFound();
            }

            var employeePosition = await _context.EmployeePositions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeePosition == null)
            {
                return NotFound();
            }

            return View(employeePosition);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployeePositions == null)
            {
                return Problem("Entity set 'PostCityContext.EmployeePositions'  is null.");
            }
            var employeePosition = await _context.EmployeePositions.FindAsync(id);
            if (employeePosition != null)
            {
                _context.EmployeePositions.Remove(employeePosition);
                _logger.LogInformation($"Delete employee position ({employeePosition.Position})");

            }
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool EmployeePositionExists(int id)
        {
          return (_context.EmployeePositions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
