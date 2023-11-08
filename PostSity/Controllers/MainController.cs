using Microsoft.AspNetCore.Mvc;
using PostSity.Data;
using System.Diagnostics.Contracts;

namespace PostSity.Controllers
{
    public class MainController : Controller
    {
        protected readonly PostCityContext _context;
        public MainController(PostCityContext context)
        {
            _context = context;
        }
    }
}
