namespace PostCity.Infrastructure.Filters
{

    public class FilterBy<T>
    {
        public IEnumerable<T> FilterByInt(
            IEnumerable<T> data,
            Func<T, int> propertySelector,
            int? filterValue)
        {
            if(filterValue != null)
                return data.Where(item => propertySelector(item) == filterValue);
            else
            {
                return data;
            }
        }

        public IEnumerable<T> FilterByDate(
            IEnumerable<T> data,
            Func<T, DateTime> propertySelector,
            DateTime? filterDate)
        {
           
            if(filterDate != null)
            {
                return data.Where(item => propertySelector(item) == filterDate);
            }
            else return data;
            
        }

        public IEnumerable<T> FilterByString(
            IEnumerable<T> data,
            Func<T, string> propertySelector,
            string filterValue)
        {
            return !string.IsNullOrEmpty(filterValue)
                ? data.Where(item => propertySelector(item).Contains(filterValue ?? ""))
                : data;
        }
        public IEnumerable<T> FilterByDecimal(
        IEnumerable<T> data,
        Func<T, decimal> propertySelector,
        decimal? filterValue,
        decimal tolerance = 1)
        {
            if (filterValue != null)
            {
                return data.Where(item => Math.Abs(propertySelector(item) - filterValue.Value) <= tolerance);
            }
            else
            {
                return data;
            }
        }

    }
}
