using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.DAL;
using ProjektDyplomowy.Entities;
using System.Text.RegularExpressions;

namespace ProjektDyplomowy.Repositories
{
    public class PostsRepository : BaseRepository<Post>, IPostsRepository
    {
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment hostEnvironment;

        public PostsRepository(AppDbContext context, IMapper mapper, IWebHostEnvironment hostEnvironment) : base(context)
        {
            this.mapper = mapper;
            this.hostEnvironment = hostEnvironment;
        }

        public Task<List<Post>> GetAllPostsAsync()
        {
            IQueryable<Post> posts = context.Posts
                .Include(u => u.User)
                .Include(t => t.Tags)
                .Include(c => c.Category);

            return posts.ToListAsync();
        }

        public Task<Post> GetPostByIdAsync(Guid id, string sortComBy = "date", bool commentsIncluded = false)
        {
            IQueryable<Post> posts = context.Posts
                .Include(u => u.User)
                .Include(t => t.Tags)
                .Include(c => c.Category)
                .Include(ul => ul.UsersWhoLikePost);

            if (commentsIncluded)
            {
                if (sortComBy == "date")
                {
                    posts = posts.Include(com => com.Comments.OrderByDescending(cd => cd.CreationDate))
                        .ThenInclude(u => u.User);
                    posts = posts.Include(com => com.Comments.OrderByDescending(cd => cd.CreationDate))
                        .ThenInclude(u => u.UsersWhoLikeComment);
                }

                else
                {
                    posts = posts.Include(com => com.Comments.OrderByDescending(com => com.LikesQuantity))
                        .ThenInclude(u => u.User);
                    posts = posts.Include(com => com.Comments.OrderByDescending(com => com.LikesQuantity))
                        .ThenInclude(u => u.UsersWhoLikeComment);
                }
            }

            return posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<SelectListItem>> FillCategoriesSelectListAsync()
        {
            var categories = await context.Categories.OrderBy(o => o.Name).ToListAsync();

            return mapper.Map<List<SelectListItem>>(categories);
        }

        public string UploadFile(IFormFile file, string title)
        {
            var fileName = Path.GetFileName(file.FileName);
            var uniqueFileName = $"{Regex.Replace(title, @"\s+", "")}_{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(fileName)}";

            var uploads = Path.Combine(hostEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, uniqueFileName);
            file.CopyTo(new FileStream(filePath, FileMode.Create));

            return uniqueFileName;
        }
    }
}
