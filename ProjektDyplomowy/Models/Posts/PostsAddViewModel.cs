using Microsoft.AspNetCore.Mvc.Rendering;
using ProjektDyplomowy.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Models.Posts
{
    public class PostsAddViewModel
    {
        [Required(ErrorMessage = "Pole tytuł jest wymagane.")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Opis (opcjonalnie)")]
        [MaxLength(500, ErrorMessage = "Opis nie może zawierać wiecej niż 500 znaków")]
        public string Text { get; set; }

        [Display(Name = "Plik")]
        public IFormFile File { get; set; }

        [Display(Name = "Adres URL")]
        public string FileUrl { get; set; }

        [Required(ErrorMessage = "Pole typ zawartości jest wymagane.")]
        [Display(Name = "Typ zawartości")]
        public ContentType ContentType { get; set; }

        [Display(Name = "Typ zawartości")]
        public List<string> Tags { get; set; }

        [Required(ErrorMessage = "Pole kategoria jest wymagane.")]
        [Display(Name = "Kategoria")]
        public Guid CategoryId { get; set; }

        public List<SelectListItem> SelectCategories { get; set; }

        public int FileSource { get; set; }
        public int FileType { get; set; }
    }
}
