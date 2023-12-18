using Domains.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Repository;
using System.Diagnostics;


namespace PostCity.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<PostCityUser> _userManager;
        private readonly PostCityContext _context;
        private readonly SessionLogger _logger;

        public HomeController(UserManager<PostCityUser> userManager, PostCityContext context, SessionLogger logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            PostCityUser currentUser = _userManager.GetUserAsync(User).Result;

            if (currentUser != null)
            {
                ViewData["Email"] = currentUser.Email;

                var roles = _userManager.GetRolesAsync(currentUser).Result;
                var userRole = roles.FirstOrDefault();

                ViewData["Role"] = userRole;
                ViewData["UserId"] = currentUser.UserId;

                if (userRole == "Employee")
                {
                    Employee employee = _context.Employees.Find(currentUser.UserId);
                    if (employee != null)
                    {
                        ViewData["Name"] = employee.FullName;
                        ViewData["Office"] = employee.OfficeId;
                        ViewData["Position"] = employee.Position;
                    }
                }
                else if (userRole == "Recipient")
                {
                    Recipient recipient = _context.Recipients.Find(currentUser.UserId);
                    if (recipient != null)
                    {
                        ViewData["Name"] = recipient.FullName;
                        ViewData["Phone"] = recipient.MobilePhone;
                    }
                }
                else if (userRole == "Admin")
                {

                }
            }

            List<string> logs = _logger.GetSessionLogs();
            return View(logs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
