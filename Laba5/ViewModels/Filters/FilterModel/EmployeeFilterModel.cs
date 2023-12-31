﻿using PostCity.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Laba4.ViewModels.Filters.FilterModel;

namespace PostCity.ViewModels.Filters.FilterModel
{
    public class EmployeeFilterModel : ITableFilterModel
    {
        public string? Name { get; set; }
        public string? Middlename { get; set; }
        public string? Surname { get; set; }
        public string? Office { get; set; }
        public string? Position { get; set; }
    }
}
