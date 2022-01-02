using ProjektDyplomowy.Entities;

namespace ProjektDyplomowy.Repositories
{
    public interface ICategoriesRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(Guid categoryId, bool includePosts = false);
    }
}
