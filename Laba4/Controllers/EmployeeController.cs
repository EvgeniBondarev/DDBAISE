using Laba4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laba4.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly SubsCityContext _context;

        public EmployeeController(SubsCityContext subsCityContext)
        {
            _context = subsCityContext;
        }

        [ResponseCache(CacheProfileName = "DbTableCache")]
        public IActionResult ShowEmployee()
        {
            return View(_context.Employees.Include(p => p.Position).Include(o => o.Office));
        }
    }
}
