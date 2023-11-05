namespace PostSity.Controllers
{
    public interface IModelController <T>
    {
        IEnumerable<T> ShowTable();
        IEnumerable<T> ShowTable(string filter);
        T ShowTable(int Id);
    }
}
