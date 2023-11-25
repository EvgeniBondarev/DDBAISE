using Laba4.Models;
using Laba4.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laba4.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<PostCityUser> _userManager;
        private readonly PostCityContext _context;

        public UserController(RoleManager<IdentityRole> roleManager, UserManager<PostCityUser> userManager, PostCityContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync(); 

            var usersList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var roleToDisplay = roles.FirstOrDefault();

                IUser userObj = null;

                if (roleToDisplay == "Recipient")
                {
                    userObj = await _context.Recipients.FindAsync(user.UserId);
                }
                else if (roleToDisplay == "Employee")
                {
                    userObj = await _context.Employees.FindAsync(user.UserId);
                }
                

                usersList.Add(new UserViewModel()
                {
                    Id = user.Id,
                    UserObj = userObj,
                    UserId = user.UserId,
                    Role = roleToDisplay,
                    Email = user.Email,
                });
            }

            return View(usersList);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            PostCityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Roles() => View(_roleManager.Roles);

    }
}
