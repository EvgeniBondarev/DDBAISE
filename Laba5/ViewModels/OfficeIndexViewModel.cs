using PostCity.Models;
using PostCity.ViewModels.Filters;
using PostCity.ViewModels.Filters.FilterModel;

namespace PostCity.ViewModels
{
    public class OfficeIndexViewModel : IndexViewModel<Office>
    {
        public OfficeFilterModel OfficeFilter { get; set; }
        public OfficeIndexViewModel(IEnumerable<Office> model, PageViewModel pageViewModel) : base(model, pageViewModel)
        {
        }
    }
}
