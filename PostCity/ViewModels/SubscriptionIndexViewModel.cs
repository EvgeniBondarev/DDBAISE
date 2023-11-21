using PostCity.Models;
using PostCity.ViewModels.Filters;

namespace PostCity.ViewModels
{
    public class SubscriptionIndexViewModel : IndexViewModel<Subscription>
    {
        public SubscriptionFilterModel SubscriptionFilter { get; set; }

        public SubscriptionIndexViewModel(IEnumerable<Subscription> subscriptions, PageViewModel pageViewModel)
            : base(subscriptions, pageViewModel)
        {
        }
    }
}

