using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Laba4.Models
{
    public class PostCityUser : IdentityUser
    {
        [Required]
        public int UserId { get; set; }
    }
}
