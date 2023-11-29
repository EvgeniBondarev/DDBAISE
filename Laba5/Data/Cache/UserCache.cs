using Laba4.Models;
using Laba4.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PostCity.Data.Cache;
using PostCity.Models;

namespace Laba4.Data.Cache
{
    public class UserCache : IAppCache
    {
        private readonly UserManager<PostCityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly int _saveTime = 2 * 2 * 240;
        private readonly PostCityContext _context;
        private readonly IMemoryCache _cache;

        public UserCache(PostCityContext db, IMemoryCache memoryCache, 
                        UserManager<PostCityUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _context = db;
            _cache = memoryCache;
            _userManager = userManager;
            _roleManager = roleManager;
            _roleManager = roleManager;
        }

        public IEnumerable<UserViewModel> Get()
        {
            _cache.TryGetValue("Users", out IEnumerable<UserViewModel>? users);

            if (users is null)
            {
                users = Set();
            }

            return users;
        }


        public IEnumerable<UserViewModel> Set()
        {
            var users = _userManager.Users.ToList();
            var usersList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
                var roleToDisplay = roles.FirstOrDefault();
                IUser userObj = null;

                if (roleToDisplay == "Recipient")
                {
                    userObj = _context.Recipients.Find(user.UserId);
                }
                else if (roleToDisplay == "Employee")
                {
                    userObj = _context.Employees.Find(user.UserId);
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

            _cache.Set("Users", usersList, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return usersList;
        }


        public IEnumerable<UserViewModel> Set(IEnumerable<UserViewModel> users)
        {
            _cache.Set("Users", users, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return users;
        }

        public void Update()
        {
            Set();
        }
    }
}
