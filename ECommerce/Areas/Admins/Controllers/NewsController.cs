using ECommerce.Data;
using ECommerce.Models.News;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ECommerce.Extension;
using ECommerce.Entities;
using System;
using ECommerce.Constants;

namespace ECommerce.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Route("admins/news")]
    public class NewsController : Controller
    {
        private readonly EcommerceDbContext dbContext;
        private readonly IStorageService storageService;

        public NewsController(EcommerceDbContext dbContext, IStorageService storageService)
        {
            this.dbContext = dbContext;
            this.storageService = storageService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(int? currentPage)
        {
            if (currentPage == null || currentPage == 0)
                currentPage = 1;

            var newsQuery = from n in dbContext.News
                            orderby n.CreationTime descending
                            select new NewsViewModel { 
                                Id = n.Id,
                                Summary = n.Summary,
                                Image = n.Image,
                                Url = n.Url,
                                Title = n.Title
                            };

            var news = await newsQuery.ToPagedListAsync<NewsViewModel>((int)currentPage, 5);
            return View(news);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateNewsModel model)
        {
            //Validate
            var hasError = false;
            if (model.Image == null)
            {
                hasError = true;
                ViewBag.Error += "Vui lòng chọn hình ảnh";
            }

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                hasError = true;
                ViewBag.Error += ", nhập tiêu đề";
            }

            if (string.IsNullOrWhiteSpace(model.Summary))
            {
                hasError = true;
                ViewBag.Error += ", nhập tóm tắt";
            }

            if (string.IsNullOrWhiteSpace(model.Content))
            {
                hasError = true;
                ViewBag.Error += ", nhập mô tả";
            }

            if (string.IsNullOrWhiteSpace(model.Url))
            {
                hasError = true;
                ViewBag.Error += ", nhập url";
            }

            if (hasError)
            {
                ViewBag.Error += " cho sản phẩm";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var news = new NewsEntity
                {
                    Title = model.Title,
                    Summary = model.Summary,
                    Content = model.Content,
                    CreationTime = DateTime.UtcNow,
                    Url = model.Url
                };

                if (model.Image != null)
                {
                    var thumbnailImageUrl = await storageService.SaveFileAsync(model.Image,
                        UploadPathConstant.NewsPath);
                    news.Image = thumbnailImageUrl;
                }

                dbContext.Add(news);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }


        [HttpGet]
        [Route("edit")]
        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsQuery = from n in dbContext.News
                            orderby n.CreationTime descending
                            select new EditNewsViewModel
                            {
                                Id = n.Id,
                                Summary = n.Summary,
                                Image = n.Image,
                                Url = n.Url,
                                Title = n.Title,
                                Content = n.Content
                            };

            var news = await newsQuery.Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        [Route("edit")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EditAsync(EditNewsModel newsModel)
        {
            //Validate
            var hasError = false;

            if (string.IsNullOrWhiteSpace(newsModel.Title))
                hasError = true;

            if (string.IsNullOrWhiteSpace(newsModel.Summary))
                hasError = true;

            if (string.IsNullOrWhiteSpace(newsModel.Url))
                hasError = true;

            if (string.IsNullOrWhiteSpace(newsModel.Content))
                hasError = true;

            if (hasError)
            {
                return RedirectToAction("edit", new { id = newsModel.Id });
            }

            var news = await dbContext.News.Where(x => x.Id == newsModel.Id)
                    .FirstOrDefaultAsync();

            news.Title = newsModel.Title;
            news.Summary = newsModel.Summary;
            news.Content = newsModel.Content;
            news.Url = newsModel.Url;

            if (newsModel.Image != null)
            {
                await storageService.DeleteFileAsync(news.Image);
                var thumbnailImageUrl = await storageService.SaveFileAsync(newsModel.Image,
                    UploadPathConstant.NewsPath);
                news.Image = thumbnailImageUrl;
            }

            dbContext.Update(news);

            await dbContext.SaveChangesAsync();
            return RedirectToAction("edit", new { id = newsModel.Id });
        }


        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            var news = await dbContext.News.FindAsync(id);
            dbContext.News.Remove(news);
            await storageService.DeleteFileAsync(news.Image);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}