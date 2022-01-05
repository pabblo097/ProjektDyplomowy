using ProjektDyplomowy.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Models.Posts
{
    public class PagedPostsSearchViewModel : PagedResultBase
    {
        public List<PostsIndexViewModel> Posts { get; set; }
        [Display(Name = "Szukaj")]
        public string SearchTerm { get; set; }
        [Display(Name = "Sposób wyszukiwania")]
        public SearchType SearchType { get; set; }
    }
}
