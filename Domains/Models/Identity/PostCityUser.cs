using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domains.Models
{
    public class PostCityUser : IdentityUser
    {
        [Required]
        public int UserId { get; set; }
    }
}
