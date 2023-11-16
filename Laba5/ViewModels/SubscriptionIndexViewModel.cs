using Laba4.Models;
using Laba4.ViewModels.Filters;

namespace Laba4.ViewModels
{
    public class SubscriptionIndexViewModel
    {
        public SubscriptionFilterModel SubscriptionFilter;
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
