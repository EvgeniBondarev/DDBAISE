using Laba4.Models;
using Laba4.ViewModels.Filters;

namespace Laba4.ViewModels
{
    public class PublicationIndexViewModel
    {
        public PublicationFilterModel publicationFilter;
        public IEnumerable<Publication> Publications { get; }
        public PageViewModel PageViewModel { get; }
        public PublicationIndexViewModel(IEnumerable<Publication> publications, 
                                         PageViewModel viewModel)
        {
            Publications = publications;
            PageViewModel = viewModel;
        }
    }
}
