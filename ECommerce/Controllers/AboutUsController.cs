using ECommerce.Data;
using ECommerce.Models.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly EcommerceDbContext dbContext;

        public AboutUsController(EcommerceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("gioithieu.html")]
        public async Task<IActionResult> AboutUs()
        {
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

            ViewBag.DisplaySlider = false;
            ViewBag.LatestNews = await latestNewsQuery.Take(5).ToListAsync();

            ViewBag.Title = "Gchill";
            ViewBag.Description = "Gchill nơi có mọi thứ bạn cần, hãy để chúng tôi phục vụ bạn";
            return View();
        }
    }
}