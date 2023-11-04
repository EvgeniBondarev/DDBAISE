using Laba4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laba4.Controllers
{
    public class OfficeController : Controller
    {
        private readonly SubsCityContext _context;

        public OfficeController(SubsCityContext context)
        {
            _context = context;
        }

        [ResponseCache(CacheProfileName = "DbTableCache")]
        public IActionResult ShowOffice()
        {
            return View(_context.Offices);
        }
    }
}
