using PostCity.Models;
using PostCity.ViewModels.Filters;
using PostCity.ViewModels.Filters.FilterModel;

namespace PostCity.ViewModels.Sort
{
    public class EmployeeIndexViewModel : IndexViewModel<Employee>
    {
        public EmployeeFilterModel EmployeeFilterModel { get; set; }
        public EmployeeIndexViewModel(IEnumerable<Employee> employees, PageViewModel pageViewModel)
            : base(employees, pageViewModel)
        {
        }
    }
}
