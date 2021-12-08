using System.ComponentModel.DataAnnotations;

namespace ProjektDyplomowy.Entities
{
    public enum ContentType
    {
        [Display(Name = "Zdjęcie/Gif")]
        ZdjecieGif,
        Wideo
    }
}
