using ECommerce.Data;
using ECommerce.Extension;
using ECommerce.Models.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    public class NewsController : Controller
    {
        private readonly EcommerceDbContext dbContext;

        public NewsController(EcommerceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("tintuc.html")]
        public async Task<IActionResult> NewsListAsync(int? currentPage)
        {
            if (currentPage == null || currentPage == 0)
                currentPage = 1;

            var newsQuery = from n in dbContext.News
                            orderby n.CreationTime descending
                            select new NewsViewModel
                            {
                                Id = n.Id,
                                Summary = n.Summary,
                                Image = n.Image,
                                Url = n.Url,
                                Title = n.Title,
                                CreateDate = n.CreationTime.ToString("dd/MM/yyyy")
                            };

            ViewBag.DisplaySlider = false;
            ViewBag.Title = "Gchill tin tức";
            ViewBag.Description = "Theo dõi tin tức mới nhất cùng Gchill, hãy để chúng tôi phục vụ bạn";
            var news = await newsQuery.ToPagedListAsync<NewsViewModel>((int)currentPage, 10);
            return View(news);
        }

        [HttpGet("tintuc/{url}")]
        public async Task<IActionResult> DetailAsync(string url)
        {
            if (url == null)
            {
                return NotFound();
            }

            var newsQuery = from n in dbContext.News
                                orderby n.CreationTime descending
                                where n.Url == url
                                select new NewsDetailModel
                                {
                                    Id = n.Id,
                                    Summary = n.Summary,
                                    Image = n.Image,
                                    Url = n.Url,
                                    Title = n.Title,
                                    CreateDate = n.CreationTime.ToString("dd/MM/yyyy"),
                                    Content = n.Content
                                };

            var latestNewsQuery = from n in dbContext.News
                            orderby n.CreationTime descending
                            select new NewsDetailModel
                            {
                                Id = n.Id,
                                Summary = n.Summary,
                                Image = n.Image,
                                Url = n.Url,
                                Title = n.Title,
                                CreateDate = n.CreationTime.ToString("dd/MM/yyyy"),
                                Content = n.Content
                            };

            var newsDetail = await newsQuery.FirstOrDefaultAsync();

            ViewBag.Title = newsDetail.Title;
            ViewBag.Description = newsDetail.Summary;
            ViewBag.DisplaySlider = false;
            ViewBag.LatestNews = await latestNewsQuery.Take(5).ToListAsync();

            if (newsDetail == null)
            {
                return NotFound();
            }

            return View(newsDetail);
        }
    }
}
