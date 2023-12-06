using System.ComponentModel.DataAnnotations;

namespace Domains.Models
{
    public class PublicationType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; } = null!;
        public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
    }
}
