using Laba4.Models;

namespace Laba4.ViewModels
{
    public class SubscriptionIndexViewModel
    {
        public string StandardDuration;
        public string StandardSubscriptionStartDate;
        public string StandardPublicationName;
        public string StandardPublicationPrice;
        public string StandardPublicationType;
        public IEnumerable<Subscription> Subscription { get; }
        public PageViewModel PageViewModel { get; }
        public SubscriptionIndexViewModel(IEnumerable<Subscription> subscriptions, 
                                         PageViewModel viewModel)
        {
            Subscription = subscriptions;
            PageViewModel = viewModel;
        }
    }
}
