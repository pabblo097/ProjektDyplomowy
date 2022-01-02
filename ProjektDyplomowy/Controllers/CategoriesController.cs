using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektDyplomowy.Entities;
using ProjektDyplomowy.Models;
using ProjektDyplomowy.Repositories;

namespace ProjektDyplomowy.Controllers
{
    [Authorize(Roles = "Admin")]
    [Controller]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IMapper mapper;
        private readonly IPostsRepository postsRepository;

        public CategoriesController(ICategoriesRepository categoriesRepository, IMapper mapper, IPostsRepository postsRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.mapper = mapper;
            this.postsRepository = postsRepository;
        }

        public async Task<IActionResult> Manage()
        {
            var categories = await categoriesRepository.GetAllCategoriesAsync();

            if (!categories.Any())
                return RedirectToAction("Error404", "Error");

            return View(categories);
        }

        public IActionResult Add() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var allCategories = await categoriesRepository.GetAllCategoriesAsync();

                foreach (var category in allCategories)
                {
                    if (category.Name == model.Name)
                    {
                        ModelState.AddModelError("NameTaken", "Ta nazwa jest zajęta.");
                        return View(model);
                    }
                }

                var newCategory = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name
                };

                if (!await categoriesRepository.AddAsync(newCategory))
                    return RedirectToAction("Error", "Home");

                TempData["SuccessAlert"] = "Pomyślnie dodano kategorie.";

                return RedirectToAction("Manage");
            }
            else
                return View(model);
        }

        public async Task<IActionResult> Edit(Guid categoryId)
        {
            var category = await categoriesRepository.GetCategoryByIdAsync(categoryId);

            if (category == null)
                return RedirectToAction("Error404", "Error");

            var categoryViewModel = mapper.Map<CategoryViewModel>(category);

            return View(categoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var allCategories = await categoriesRepository.GetAllCategoriesAsync();

                foreach (var category in allCategories)
                {
                    if (category.Name == model.Name)
                    {
                        ModelState.AddModelError("NameTaken", "Ta nazwa jest zajęta.");
                        return View(model);
                    }
                }

                var editedCategory = await categoriesRepository.GetCategoryByIdAsync(model.Id);

                mapper.Map(model, editedCategory);

                if (!await categoriesRepository.UpdateAsync(editedCategory))
                    return RedirectToAction("Error", "Home");

                TempData["SuccessAlert"] = "Pomyślnie zedytowano kategorie.";

                return RedirectToAction("Manage");
            }
            else
                return View(model);
        }

        [HttpPost("[controller]/[action]/{categoryId}")]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            var category = await categoriesRepository.GetCategoryByIdAsync(categoryId, true);

            if (category == null)
                return RedirectToAction("Error404", "Error");

            foreach (var post in category.Posts)
            {
                postsRepository.RemoveFile(post);
            }

            if (!await categoriesRepository.RemoveAsync(category))
                return RedirectToAction("Error", "Home");

            TempData["SuccessAlert"] = "Pomyślnie usunięto kategorie.";
            return RedirectToAction("Manage");
        }
    }
}
