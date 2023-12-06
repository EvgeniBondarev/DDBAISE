

namespace Domains.Models
{
    public class ChangeRoleViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public IEnumerable<Microsoft.AspNetCore.Identity.IdentityRole> AllRoles { get; set; }


    }
}
