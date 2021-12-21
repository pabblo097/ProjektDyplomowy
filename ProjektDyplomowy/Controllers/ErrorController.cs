using Microsoft.AspNetCore.Mvc;

namespace ProjektDyplomowy.Controllers
{
    public class ErrorController : Controller
    {
        [Route("[controller]/404")]
        public IActionResult Error404()
        {
            return View("404");
        }
    }
}
