using Microsoft.AspNetCore.Mvc.Rendering;
using ProjektDyplomowy.Entities;

namespace ProjektDyplomowy.Repositories
{
    public interface IPostsRepository : IBaseRepository<Post>
    {
        Task<List<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(Guid id);
        Task<List<SelectListItem>> FillCategoriesSelectListAsync();
        string UploadFile(IFormFile file, string title);
    }
}
