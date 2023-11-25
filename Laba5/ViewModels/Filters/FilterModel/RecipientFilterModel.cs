using Microsoft.AspNetCore.Mvc.ApplicationModels;
using PostCity.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Laba4.ViewModels.Filters.FilterModel
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
