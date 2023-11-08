using System.ComponentModel.DataAnnotations;

namespace PostSity.Models
{
    public class RecipientAddress
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)] 
        public string Street { get; set; }

        [Required]
        public int House { get; set; }

        public int Apartment { get; set; }

        public virtual ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();
    }
}
