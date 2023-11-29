using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostCity.Models
{
    public class PublicationType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; } = null!;

        [InverseProperty("Type")]
        [CascadeDelete]
        public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
    }
}
