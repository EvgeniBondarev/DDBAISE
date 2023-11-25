using Laba4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PostCity.Models;

namespace Laba4.Data
{
    public class UserInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<PostCityUser> _userManager;
        private readonly PostCityContext _context;

        public UserInitializer(RoleManager<IdentityRole> roleManager, UserManager<PostCityUser> userManager, PostCityContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public void Initialize()
        {
            CreateRoles().Wait();
            CreateAdmin().Wait();
            CreateEmployees().Wait();
            CreateRecipient().Wait();
        }

        private async Task CreateRoles()
        {
            string[] roleNames = { "Admin", "Employee", "Recipient" };

            foreach (var roleName in roleNames)
            {

                var roleExist = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private async Task CreateAdmin()
        {
            if (_userManager.FindByNameAsync("admin@gmail.com").Result == null)
            {
                PostCityUser user = new PostCityUser
                {
                    UserId = 1,
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com"
                };

                IdentityResult result = await _userManager.CreateAsync(user, "EQRu~ha+75hqIcr");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

        }
        private async Task CreateEmployees()
        {
            var employees = await _context.Employees.ToListAsync();

            foreach (var employee in employees)
            {
                var existingUser = await _userManager.FindByEmailAsync($"Employee{employee.Id}@gmail.com");

                if (existingUser == null)
                {
                    PostCityUser user = new PostCityUser
                    {
                        UserId = employee.Id, 
                        UserName = $"Employee{employee.Id}@gmail.com",
                        Email = $"Employee{employee.Id}@gmail.com"
                    };
                    var password = $"Employee_{employee.Id}";
                    IdentityResult result = await _userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Employee");
                    }
                }
            }
        }
        private async Task CreateRecipient()
        {
            var recipients = await _context.Recipients.ToListAsync();

            foreach (var recipient in recipients)
            {
                var existingUser = await _userManager.FindByEmailAsync($"Recipient{recipient.Id}@gmail.com");

                if (existingUser == null)
                {
                    PostCityUser user = new PostCityUser
                    {
                        UserId = recipient.Id,
                        UserName = $"Recipient{recipient.Id}@gmail.com",
                        Email = $"Recipient{recipient.Id}@gmail.com"
                    };
                    var password = $"Recipient_{recipient.Id}";
                    IdentityResult result = await _userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Recipient");
                    }
                }
            }
        }
    }

}