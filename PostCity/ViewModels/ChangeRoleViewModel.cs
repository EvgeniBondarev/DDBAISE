using Microsoft.AspNetCore.Identity;

namespace Laba4.Models
{
    public class ChangeRoleViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public IEnumerable<IdentityRole> AllRoles { get; set; }


    }
}
