using Laba4.Models;
using Laba4.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Laba4.Data
{
    public class UserRegistrationManager
    {
        private readonly UserManager<PostCityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRegistrationManager(UserManager<PostCityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> RegisterUserWithRole(PostCityUserModel  postCityUserModel)
        {
            var roleExists = await _roleManager.RoleExistsAsync(postCityUserModel.Role);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(postCityUserModel.Role));
            }

            var user = new PostCityUser { UserName = postCityUserModel.Email, 
                                          Email = postCityUserModel.Email, 
                                          UserId = postCityUserModel.UserId };

            var result = await _userManager.CreateAsync(user, postCityUserModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, postCityUserModel.Role);
            }

            return result;
        }
    }
}
