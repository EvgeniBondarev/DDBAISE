using Laba4.Models;
using Laba4.ViewModels.Filters;

namespace Laba4.ViewModels
{
    public class PublicationTypeIndexViewModel
    {
        public PublicationTypeFilterModel PublicationFilter;
        public IEnumerable<PublicationType> PublicationTypes { get; }
        public PageViewModel PageViewModel { get; }
        public PublicationTypeIndexViewModel(IEnumerable<PublicationType> types, 
                                         PageViewModel viewModel)
        {
            PublicationTypes = types;
            PageViewModel = viewModel;
        }
    }
}
