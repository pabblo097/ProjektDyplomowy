namespace ProjektDyplomowy.Models.Posts
{
    public class PagedPostsIndexViewModel : PagedResultBase
    {
        public List<PostsIndexViewModel> Posts { get; set; }
        public string SortBy { get; set; }
        public string CategoryName { get; set; }
    }
}
