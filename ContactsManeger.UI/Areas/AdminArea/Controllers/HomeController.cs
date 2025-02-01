using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.UI.Areas.AdminArea.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        [Route("admin/home/index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
