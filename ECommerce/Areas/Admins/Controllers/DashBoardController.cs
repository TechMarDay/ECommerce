using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class DashBoardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}