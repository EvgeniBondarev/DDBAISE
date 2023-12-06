namespace Domains.ViewModels.Filters.FilterModel
{
    public class UserFilterModel : ITableFilterModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
