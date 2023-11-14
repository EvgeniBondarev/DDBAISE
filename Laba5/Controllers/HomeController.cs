using Laba4.Data.Cache;
using Laba4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace Laba4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SubscriptionCache _subscriptionCache;
        private readonly PublicationCache _publicationCache;
        public HomeController(ILogger<HomeController> logger, SubscriptionCache subscriptionCache, PublicationCache publicationCache)
        {
            _logger = logger;
            _subscriptionCache = subscriptionCache;
            _publicationCache = publicationCache;

            _subscriptionCache.Set();
            _publicationCache.Set();            
        }

        public IActionResult Index()
        {
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
