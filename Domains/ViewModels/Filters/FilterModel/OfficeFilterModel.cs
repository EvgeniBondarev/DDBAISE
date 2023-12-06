namespace Domains.ViewModels.Filters.FilterModel
{
    public class OfficeFilterModel : ITableFilterModel
    {
        public string OwnerName { get; set; }
        public string OwnerMiddlename { get; set; }
        public string OwnerSurname { get; set; }
        public string StreetName { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
    }
}
