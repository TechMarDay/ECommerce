using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ECommerce.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class PostController : Controller
    {
        private IWebHostEnvironment hostingEnvironment;

        public PostController(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("Content")] PostModel model)
        {
            return View(model);
        }
    }

    public class PostModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public string Image { get; set; }

        public DateTime LastModificationTime { get; set; }
    }
}