using System.ComponentModel.DataAnnotations;

namespace Domains.Models
{
    public class Office
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string OwnerName { get; set; }

        [Required]
        [MaxLength(50)]
        public string OwnerMiddlename { get; set; }

        [Required]
        [MaxLength(50)]
        public string OwnerSurname { get; set; }

        [Required]
        [MaxLength(100)]
        public string StreetName { get; set; }

        [Required]
        [MaxLength(15)]
        [Phone]
        public string MobilePhone { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
