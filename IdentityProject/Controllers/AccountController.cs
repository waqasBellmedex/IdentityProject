using Microsoft.AspNetCore.Mvc;

namespace IdentityProject.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
