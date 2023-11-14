using Laba4.Models;

namespace Laba4.ViewModels
{
    public class PublicationIndexViewModel
    {
        public string StandardPublicationPrice;
        public string StandardPublicationType;
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
