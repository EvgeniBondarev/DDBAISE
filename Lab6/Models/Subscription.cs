using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostCity.Models
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

        [ForeignKey("EmployeeId")]
        public int? EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime SubscriptionStartDate { get; set; }

        [InverseProperty("Subscriptions")]
        [CascadeDelete]
        public Office? Office { get; set; } = null!;

        [InverseProperty("Subscriptions")]
        [CascadeDelete]
        public Publication? Publication { get; set; } = null!;

        [InverseProperty("Subscriptions")]
        [CascadeDelete]
        public Recipient? Recipient { get; set; } = null!;

        [InverseProperty("Subscriptions")]
        [CascadeDelete]
        public Employee? Employee { get; set; }
    }
}
