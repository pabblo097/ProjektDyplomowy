using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.DAL;
using ProjektDyplomowy.Entities;
using ProjektDyplomowy.Models.Posts;
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

        public async Task<PagedPostsIndexViewModel> GetAllPostsAsync(int page = 1, string sortBy = "date", string category = "none")
        {
            IQueryable<Post> posts = context.Posts
                .Include(u => u.User)
                .Include(t => t.Tags)
                .Include(c => c.Category)
                .Include(ul => ul.UsersWhoLikePost);

            if (category != "none")
            {
                posts = posts.Where(c => c.Category.Name == category);
            }

            if (sortBy == "date")
            {
                posts = posts.OrderByDescending(d => d.CreationDate);
            }
            else if (sortBy == "likes")
            {
                posts = posts.OrderByDescending(d => d.CreationDate.Date).ThenByDescending(l => l.LikesQuantity);
            }


            int size = 10;
            int skip = (page - 1) * size;
            int count = await posts.CountAsync();
            posts = posts.Skip(skip).Take(size);

            var pagedPosts = new PagedPostsIndexViewModel
            {
                Posts = mapper.Map<List<PostsIndexViewModel>>(await posts.ToListAsync()),
                CurrentPage = page,
                PageSize = size,
                AllItemsCount = count,
                SortBy = sortBy,
                CategoryName = category
            };

            return pagedPosts;
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
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public void RemoveFile(Post entity)
        {
            if (entity.SourceType == SourceType.Local)
            {
                var uploads = Path.Combine(hostEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, entity.FileName);

                File.Delete(filePath);
            }
        }

        public override Task<bool> RemoveAsync(Post entity)
        {
            RemoveFile(entity);

            return base.RemoveAsync(entity);
        }

        //public async Task<PagedPostsSearchViewModel> SearchPostsAsync(string searchTerm, SearchType searchType, int page = 1)
        //{
        //    IQueryable<Post> posts = context.Posts
        //        .Include(u => u.User)
        //        .Include(t => t.Tags)
        //        .Include(c => c.Category)
        //        .Include(ul => ul.UsersWhoLikePost);

        //    if (searchType == SearchType.Tytuły)
        //    {
        //        posts = posts.Where(p => p.Title.Contains(searchTerm));
        //    }
        //    else if (searchType == SearchType.Tagi)
        //    {
        //        posts = posts.Where(p => p.Tags.Any(t => t.Name == searchTerm));
        //    }
        //    else if (searchType == SearchType.Wszystko)
        //    {
        //        posts = posts.Where(p => p.Title.Contains(searchTerm) || p.Tags.Any(t => t.Name == searchTerm));
        //    }

        //    int size = 10;
        //    int skip = (page - 1) * size;
        //    int count = await posts.CountAsync();
        //    posts = posts.Skip(skip).Take(size);

        //    var pagedPosts = new PagedPostsSearchViewModel
        //    {
        //        Posts = mapper.Map<List<PostsIndexViewModel>>(await posts.ToListAsync()),
        //        CurrentPage = page,
        //        PageSize = size,
        //        AllItemsCount = count,
        //        SearchTerm = searchTerm,
        //        SearchType = searchType
        //    };

        //    return pagedPosts;
        //}
    }
}
