using Laba4.Models;

namespace Laba4.ViewModels
{
    public class PublicationTypeIndexViewModel
    {
        public string StandardPublicationType;
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
