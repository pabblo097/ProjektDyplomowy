using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.DAL;
using ProjektDyplomowy.Entities;

namespace ProjektDyplomowy.Repositories
{
    public class PostsRepository : BaseRepository<Post>, IPostsRepository
    {
        private readonly IMapper mapper;

        public PostsRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

        public Task<List<Post>> GetAllPostsAsync()
        {
            IQueryable<Post> posts = context.Posts
                .Include(u => u.User)
                .Include(t => t.Tags)
                .Include(c => c.Category);

            return posts.ToListAsync();
        }

        public Task<Post> GetPostByIdAsync(Guid id)
        {
            IQueryable<Post> posts = context.Posts
                .Include(u => u.User)
                .Include(t => t.Tags)
                .Include(c => c.Category)
                .Include(com => com.Comments);

            return posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<SelectListItem>> FillCategoriesSelectListAsync()
        {
            return mapper.Map<List<SelectListItem>>(await context.Categories.ToListAsync());
        }
    }
}
