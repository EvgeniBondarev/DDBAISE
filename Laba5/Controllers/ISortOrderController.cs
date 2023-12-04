using Laba4.ViewModels.Sort;
using Laba4.ViewModels;

namespace Laba4.Controllers
{
    public interface ISortOrderController<T, K>
    {
        public void SetSortOrderViewData(K sortSatate);


        public IEnumerable<T> ApplySortOrder(IEnumerable<T> data, K sortSatate);

    }
}
