using Laba4.ViewModels.Filters.FilterModel;

namespace PostCity.ViewModels.Filters
{
    public class SubscriptionFilterModel : ITableFilterModel
    {
        public int? Duration { get; set; }
        public DateTime? StartDate { get; set; }
        public string? OfficeName { get; set; }
        public string? PublicationName { get; set; }
        public string? RecipientName { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime? StartPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
    }
}
