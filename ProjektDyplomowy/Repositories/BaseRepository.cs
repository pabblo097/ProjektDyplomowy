using ProjektDyplomowy.DAL;

namespace ProjektDyplomowy.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext context;

        protected BaseRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(T entity)
        {
            context.Add(entity);
            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            context.Remove(entity);
            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            context.Update(entity);
            return (await context.SaveChangesAsync()) > 0;
        }
    }
}
