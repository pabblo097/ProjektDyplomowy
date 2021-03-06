using AspNetCore.ReCaptcha;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjektDyplomowy.Entities;
using ProjektDyplomowy.Models.Posts;
using ProjektDyplomowy.Repositories;

namespace ProjektDyplomowy.Controllers
{
    [Controller]
    public class PostsController : Controller
    {
        private readonly IPostsRepository postsRepository;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public PostsController(IPostsRepository postsRepository, IMapper mapper, UserManager<User> userManager)
        {
            this.postsRepository = postsRepository;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1, string sortBy = "likes")
        {
            var pagedPosts = await postsRepository.GetAllPostsAsync(page, sortBy);

            if (!pagedPosts.Posts.Any())
            {
                return RedirectToAction("404", "Error");
            }

            return View(pagedPosts);
        }

        [Route("[controller]/{postId}")]
        public async Task<IActionResult> Details(Guid postId, string sortComBy = "date")
        {
            var post = await postsRepository.GetPostByIdAsync(postId, sortComBy, true);
            if (post == null)
            {
                return RedirectToAction("404", "Error");
            }

            var postViewModel = mapper.Map<PostsDetailsViewModel>(post);
            postViewModel.SortCommentsBy = sortComBy;


            return View(postViewModel);
        }

        [Authorize]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Add()
        {
            var postsAddViewModel = new PostsAddViewModel
            {
                SelectCategories = await postsRepository.FillCategoriesSelectListAsync(),
                FileSource = 0,
                FileType = 0,
                Tags = new List<string>()
            };

            return View(postsAddViewModel);
        }

        private async Task<IActionResult> ReturnAddViewWithError(PostsAddViewModel model, string errorKey, string errorMessage)
        {
            ModelState.AddModelError(errorKey, errorMessage);
            model.SelectCategories = await postsRepository.FillCategoriesSelectListAsync();
            return View("Add", model);
        }

        [Authorize]
        [HttpPost("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PostsAddViewModel model)
        {
            if (ModelState.IsValid)
            {   //todo zabezpieczenie przed recznym modyfikowaniem inputow FileSource, FileType itp
                if (model.FileSource == 1)
                {
                    if (model.File == null)
                    {
                        //plik nie został wrzucony
                        return await ReturnAddViewWithError(model, "EmptyFileInput", "Plik jest wymagany.");
                    }

                    var imageExtensions = new List<string> { ".JPG", ".JPE", ".JPEG", ".BMP", ".GIF", ".PNG" };
                    var videoExtensions = new List<string> { ".MP4", ".WEBM", ".OGG" };
                    if (model.FileType == 1 && !imageExtensions.Contains(Path.GetExtension(model.File.FileName).ToUpper()))
                    {
                        //wrzucony plik nie jest plikiem graficznym a powinien
                        return await ReturnAddViewWithError(model, "NotImage", "Wrzucony plik nie jest plikiem graficznym.");
                    }
                    else if (model.FileType == 2 && !videoExtensions.Contains(Path.GetExtension(model.File.FileName).ToUpper()))
                    {
                        //wrzucony plik nie jest plikiem wideo a powinien
                        return await ReturnAddViewWithError(model, "NotVideo", "Wrzucony plik nie jest plikiem wideo.");
                    }
                }
                else if (model.FileSource == 2 && string.IsNullOrWhiteSpace(model.FileUrl)) //adres URL walidacja
                {
                    //brak adresu URL
                    return await ReturnAddViewWithError(model, "EmptyFileUrlInput", "Adres URL jest wymagany.");
                }

                var currentUser = this.User;

                var post = mapper.Map<Post>(model);
                post.Id = Guid.NewGuid();
                post.User = await userManager.GetUserAsync(currentUser);
                post.CreationDate = DateTime.Now;

                if (model.FileSource == 1)
                    post.FileName = postsRepository.UploadFile(model.File, model.Title);
                else
                    post.FileName = model.FileUrl;

                if (!await postsRepository.AddAsync(post))
                {
                    return RedirectToAction("Error", "Home");
                }

                TempData["SuccessAlert"] = "Pomyślnie dodano post.";
                return RedirectToAction("Details", new { postId = post.Id });
            }

            model.SelectCategories = await postsRepository.FillCategoriesSelectListAsync();
            return View(model);
        }

        [Authorize]
        [Route("[controller]/[action]/{postId}")]
        public async Task<IActionResult> Edit(Guid postId)
        {
            var post = await postsRepository.GetPostByIdAsync(postId);
            var user = await userManager.GetUserAsync(this.User);

            if (!await userManager.IsInRoleAsync(user, "Admin") && user != post.User)
            {
                TempData["DangerAlert"] = "Nie posiadasz uprawnień do edytowania tego postu.";
                return RedirectToAction("Details", new { postId = post.Id });
            }

            if (post == null)
            {
                return RedirectToAction("404", "Errors");
            }

            var postViewModel = mapper.Map<PostsEditViewModel>(post);
            postViewModel.SelectCategories = await postsRepository.FillCategoriesSelectListAsync();

            return View(postViewModel);
        }

        [Authorize]
        [HttpPost("[controller]/[action]/{postId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostsEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var editedPost = await postsRepository.GetPostByIdAsync(model.Id);

                mapper.Map(model, editedPost);

                if (!await postsRepository.UpdateAsync(editedPost))
                {
                    return RedirectToAction("Error", "Home");
                }

                TempData["SuccessAlert"] = "Pomyślnie zedytowano post.";
                return RedirectToAction("Details", new { postId = editedPost.Id });
            }

            model.SelectCategories = await postsRepository.FillCategoriesSelectListAsync();
            return View(model);
        }

        [Authorize]
        [HttpPost("[controller]/[action]/{postId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid postId)
        {
            var post = await postsRepository.GetPostByIdAsync(postId);
            var user = await userManager.GetUserAsync(this.User);

            if (!await userManager.IsInRoleAsync(user, "Admin") && user != post.User)
            {
                TempData["DangerAlert"] = "Nie posiadasz uprawnień do usunięcia tego postu.";
                return RedirectToAction("Details", new { postId = post.Id });
            }

            if (post == null)
            {
                return RedirectToAction("404", "Errors");
            }

            if (!await postsRepository.RemoveAsync(post))
            {
                return RedirectToAction("Error", "Home");
            }

            TempData["SuccessAlert"] = "Pomyślnie usunięto post.";
            return RedirectToAction("Index", "Posts");
        }

        [Route("Kategoria/{category}")]
        public async Task<IActionResult> Categories([FromRoute] string category, int page = 1, string sortBy = "likes")
        {
            var pagedPosts = await postsRepository.GetAllPostsAsync(page, sortBy, category);

            if (!pagedPosts.Posts.Any())
            {
                return RedirectToAction("404", "Error");
            }

            return View(pagedPosts);
        }

        [Route("Search")]
        public IActionResult Search() { return View(); }

        [Route("SearchResult")]
        public async Task<IActionResult> SearchResult(string searchTerm, SearchType searchType, int page = 1)
        {
            var pagedPosts = await postsRepository.SearchPostsAsync(searchTerm, searchType, page);

            return View(pagedPosts);
        }
    }
}
