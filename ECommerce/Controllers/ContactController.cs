using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet("lienhe.html")]
        public IActionResult Index()
        {
            ViewBag.Title = "Gchill";
            ViewBag.Description = "Gchill nơi có mọi thứ bạn cần, hãy để chúng tôi phục vụ bạn";
            ViewBag.DisplaySlider = false;
            return View();
        }
    }
}
