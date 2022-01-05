using Microsoft.AspNetCore.Mvc.Rendering;
using ProjektDyplomowy.Entities;
using ProjektDyplomowy.Models.Posts;

namespace ProjektDyplomowy.Repositories
{
    public interface IPostsRepository : IBaseRepository<Post>
    {
        Task<PagedPostsIndexViewModel> GetAllPostsAsync(int page = 1, string sortBy = "date", string category = "none");
        Task<PagedPostsSearchViewModel> SearchPostsAsync(string searchTerm, SearchType searchType, int page = 1);
        Task<Post> GetPostByIdAsync(Guid id, string sortComBy = "date", bool commentsIncluded = false);
        Task<List<SelectListItem>> FillCategoriesSelectListAsync();
        string UploadFile(IFormFile file, string title);
        void RemoveFile(Post entity);
    }
}
