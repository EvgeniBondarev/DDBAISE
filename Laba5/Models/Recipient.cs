using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PostCity.Models
{
    public class Recipient
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

        public int AddressId { get; set; }

        [Required]
        [MaxLength(15)] 
        [Phone] 
        public string MobilePhone { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [ForeignKey("AddressId")]
        public virtual RecipientAddress Address { get; set; }

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
