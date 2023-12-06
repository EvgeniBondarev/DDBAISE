using System.ComponentModel.DataAnnotations;

namespace Domains.Models
{
    public class Subscription
    {
        public int Id { get; set; }

        [Required]
        public int RecipientId { get; set; }

        [Required]
        public int PublicationId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Duration { get; set; }

        [Required]
        public int OfficeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime SubscriptionStartDate { get; set; }

        public Office? Office { get; set; } = null!;
        public Publication? Publication { get; set; } = null!;
        public Recipient? Recipient { get; set; } = null!;
    }
}
