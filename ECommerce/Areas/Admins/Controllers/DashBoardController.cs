using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Route("admin")]
    public class DashBoardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}