using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Models
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Pole nazwa jest wymagane.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
    }
}
