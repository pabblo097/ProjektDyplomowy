namespace ProjektDyplomowy.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<bool> AddAsync(T entity);
        Task<bool> RemoveAsync(T entity);
        Task<bool> UpdateAsync(T entity);
    }
}
