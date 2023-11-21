using Microsoft.AspNetCore.Identity;

namespace Laba4.Data
{
    public class UserInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserInitializer(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            CreateRoles().Wait();
            CreateUsers().Wait();
        }

        private async Task CreateRoles()
        {
            string[] roleNames = { "Admin", "Manager", "User" };

            foreach (var roleName in roleNames)
            {

                var roleExist = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private async Task CreateUsers()
        {
            if (_userManager.FindByNameAsync("admin@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "Admin",
                    Email = "admin@gmail.com"
                };

                IdentityResult result = await _userManager.CreateAsync(user, "admin");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            }
        }
}
