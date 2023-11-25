
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using PostCity.Data.Cache;
using PostCity.Models;
using System.Diagnostics;

namespace Laba4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;      
        }

        public IActionResult Index()
        {
            string result = "";
            if (Request.Cookies.TryGetValue("UserCredentials", out string credentialsJson))
            {
                var credentials = JsonConvert.DeserializeAnonymousType(credentialsJson, new { Email = "", Password = "" });

                result = $"Email: {credentials.Email},\nPassword: {credentials.Password}";
            }
            else
            {
                result = "Cookie с именем 'UserCredentials' не найдена.";
            }
            ViewBag.Result = result;

            return View();
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
