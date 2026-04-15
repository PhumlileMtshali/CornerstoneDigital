using Microsoft.AspNetCore.Mvc;

namespace CornerstoneDigital.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
