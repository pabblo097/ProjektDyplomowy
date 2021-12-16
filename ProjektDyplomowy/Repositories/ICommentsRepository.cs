using ProjektDyplomowy.Entities;

namespace ProjektDyplomowy.Repositories
{
    public interface ICommentsRepository : IBaseRepository<Comment>
    {
        Task<List<Comment>> GetAllCommentsByPostIdAsync(Guid postId, string sortBy);
        Task<Comment> GetCommentByIdAsync(Guid commentId);
    }
}
