using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.DAL;
using ProjektDyplomowy.Entities;

namespace ProjektDyplomowy.Repositories
{
    public class CommentsRepository : BaseRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<Comment>> GetAllCommentsByPostIdAsync(Guid postId, string sortBy)
        {
            IQueryable<Comment> query = context.Comments
                .Where(c => c.PostId == postId);

            if (sortBy == "date")
                query.OrderBy(c => c.CreationDate);
            else
                query.OrderBy(c => c.LikesQuantity);

            return query.ToListAsync();
        }

        public Task<Comment> GetCommentByIdAsync(Guid commentId)
        {
            var comment = context.Comments.Include(ul => ul.UsersWhoLikeComment);

            return comment.FirstOrDefaultAsync(c => c.Id == commentId);
        }
    }
}
