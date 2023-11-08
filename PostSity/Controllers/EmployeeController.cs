using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostSity.Data;

namespace PostSity.Controllers
{
    public class EmployeeController : MainController, IModelController
    {
        public EmployeeController(PostCityContext context) : base(context)
        {
        }

        public IActionResult ShowTable()
        {
            return View(_context.Employees.Include(ep => ep.Position).Include(o => o.Office));
        }

    }
}
