using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Models.Comments
{
    public class CommentsAddViewModel
    {
        [Required(ErrorMessage = "Nie można dodać pustego komentarza")]
        public string Content { get; set; }
    }
}
