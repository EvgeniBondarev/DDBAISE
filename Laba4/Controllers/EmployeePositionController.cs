using Laba4.Models;
using Microsoft.AspNetCore.Mvc;

namespace Laba4.Controllers
{
    public class EmployeePositionController : Controller
    {
       private readonly SubsCityContext _context;

       public EmployeePositionController(SubsCityContext context)
       {
         _context = context;
       }

        [ResponseCache(CacheProfileName = "DbTableCache")]
        public IActionResult ShowEmployeePosition()
        {
                return View(_context.EmployeePositions);
        }
    }
}
