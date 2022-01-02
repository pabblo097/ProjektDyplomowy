using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.DAL;
using ProjektDyplomowy.Entities;

namespace ProjektDyplomowy.Repositories
{
    public class CategoriesRepository : BaseRepository<Category>, ICategoriesRepository
    {
        public CategoriesRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public Task<Category> GetCategoryByIdAsync(Guid categoryId, bool includePosts = false)
        {
            IQueryable<Category> category = context.Categories;

            if (includePosts)
                category = category.Include(c => c.Posts);

            return category.FirstOrDefaultAsync(c => c.Id == categoryId);
        }
    }
}
