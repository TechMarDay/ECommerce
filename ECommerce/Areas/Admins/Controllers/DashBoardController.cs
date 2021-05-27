using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class DashBoardController : Controller
    {
        [HttpGet("admins")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
