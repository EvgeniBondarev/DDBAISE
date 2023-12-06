using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models
{
    public class Publication
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

        public virtual PublicationType? Type { get; set; } = null!;
        public override string ToString()
        {
            return $"{Name}\t{Type.Type}\t{Price} руб.";

        }
    }
}
