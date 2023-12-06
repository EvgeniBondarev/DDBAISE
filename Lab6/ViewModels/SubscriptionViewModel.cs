using Laba4.Models;

namespace Lab6.ViewModels
{
    public class SubscriptionViewModel
    {
        public int Id { get; set; }

        public int RecipientId { get; set; }

        public int PublicationId { get; set; }

        public int Duration { get; set; }

        public int OfficeId { get; set; }

        public int? EmployeeId { get; set; }

        public string SubscriptionStartDate { get; set; } = null!;

        public Office Office { get; set; } = null!;

        public Publication Publication { get; set; } = null!;

        public Recipient Recipient { get; set; } = null!;

        public Employee Employee { get; set; } = null;
    }
}
