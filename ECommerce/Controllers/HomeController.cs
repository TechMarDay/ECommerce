using ECommerce.Data;
using ECommerce.Extension;
using ECommerce.Models;
using ECommerce.Models.News;
using ECommerce.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EcommerceDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, EcommerceDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productsQuery = from p in dbContext.Products
                                orderby p.CreationTime descending
                                select new ProductViewModel
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Price = p.Price,
                                    Discount = p.Discount,
                                    BestSeller = (bool)p.BestSeller,
                                    ThumbnailImage = p.Image,
                                    Url = p.Url,
                                    DiscountPrice = p.Price * (100 - p.Discount)/100
                                };

            ViewBag.Products = await productsQuery.Take(5).ToListAsync();

            ViewBag.BetsellerProducts = await productsQuery.Where(x => x.BestSeller)
                .Take(5).ToListAsync();

            var newsQuery = from n in dbContext.News
                            orderby n.CreationTime descending
                            select new NewsViewModel
                            {
                                Id = n.Id,
                                Summary = n.Summary,
                                Image = n.Image,
                                Url = n.Url,
                                Title = n.Title
                            };

            ViewBag.News = await newsQuery.Take(4).ToListAsync();

            ViewBag.Title = "Gchill";
            ViewBag.Description = "Gchill nơi có mọi thứ bạn cần, hãy để chúng tôi phục vụ bạn";

            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            ViewBag.Title = "Gchill";
            ViewBag.Description = "Gchill nơi có mọi thứ bạn cần, hãy để chúng tôi phục vụ bạn";
            return View();
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.Title = "Gchill";
            ViewBag.Description = "Gchill nơi có mọi thứ bạn cần, hãy để chúng tôi phục vụ bạn";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
