namespace ProjektDyplomowy.Models
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int AllItemsCount { get; set; }
        public int PageSize { get; set; }
    }
}
