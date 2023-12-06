using System.ComponentModel.DataAnnotations;

namespace Domains.Models
{
    public class Recipient : IUser
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Middlename { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        public int? AddressId { get; set; }

        [Required]
        [MaxLength(15)]
        [Phone]
        public string MobilePhone { get; set; }

        public virtual RecipientAddress? Address { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

        public override string ToString()
        {
            return $"{Surname} {Name}";
        }

        public string FullName
        {
            get { return $"{Surname} {Name} {Middlename}"; }
        }
    }
}
