﻿namespace Domains.ViewModels.Filters.FilterModel
{
    public class RecipientFilterModel : ITableFilterModel
    {
        public string? Name { get; set; }
        public string? Middlename { get; set; }
        public string? Surname { get; set; }
        public string? MobilePhone { get; set; }
        public string? Address { get; set; }
    }
}