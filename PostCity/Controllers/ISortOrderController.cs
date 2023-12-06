namespace PostCity.Controllers
{
    public interface ISortOrderController<T, K>
    {
        public void SetSortOrderViewData(K sortSatate);


        public IEnumerable<T> ApplySortOrder(IEnumerable<T> data, K sortSatate);

    }
}
