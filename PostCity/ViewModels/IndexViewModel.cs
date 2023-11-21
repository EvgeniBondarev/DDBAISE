using PostCity.Models;

namespace PostCity.ViewModels
{
    public class IndexViewModel<T>
    {
        public IEnumerable<T> Model { get; }
        public PageViewModel PageViewModel { get; }

        public IndexViewModel(IEnumerable<T> model, PageViewModel pageViewModel)
        {
            Model = model;
            PageViewModel = pageViewModel;
        }
    }
}
