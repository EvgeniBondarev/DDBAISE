using Laba4.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Laba4.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public IUser UserObj { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

    }
}
