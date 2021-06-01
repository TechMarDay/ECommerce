using ECommerce.Data;
using ECommerce.Extension;
using ECommerce.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly EcommerceDbContext dbContext;

        public ProductController(EcommerceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("{url}")]
        public async Task<IActionResult> DetailAsync(string url)
        {
            if (url == null)
            {
                return NotFound();
            }

            var productsQuery = from p in dbContext.Products
                                join c in dbContext.ProductCategories
                                     on p.CategoryId equals c.Id
                                     orderby p.CreationTime descending
                                where p.Url == url
                                select new ProductDetailModel
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Price = p.Price,
                                    Discount = p.Discount,
                                    BestSeller = (bool)p.BestSeller,
                                    ThumbnailImage = p.Image,
                                    Url = p.Url,
                                    Category = c.Name,
                                    Description  = p.Description,
                                    DiscountPrice = p.Price * (100 - p.Discount) / 100,
                                    SavePrice = p.Price - p.Price * (100 - p.Discount) / 100
                                };

            var productDetail = await productsQuery.FirstOrDefaultAsync();

            var relatedProductsQuery = from p in dbContext.Products
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
                                    DiscountPrice = p.Price * (100 - p.Discount) / 100
                                };

            ViewBag.Products = await relatedProductsQuery.Take(5).ToListAsync();

            ViewBag.Title = productDetail.Name;
            if (productDetail.Description.Length > 50)
                ViewBag.Description = productDetail.Description.Substring(0, 50);
            else
                ViewBag.Description = productDetail.Description;
            ViewBag.DisplaySlider = false;

            if (productDetail == null)
            {
                return NotFound();
            }

            return View(productDetail);
        }


        [HttpGet("sanpham.html")]
        public async Task<IActionResult> ProductListAsync(int? currentPage)
        {
            if (currentPage == null || currentPage == 0)
                currentPage = 1;

            var productsQuery = from p in dbContext.Products
                                join c in dbContext.ProductCategories
                                     on p.CategoryId equals c.Id
                                orderby p.CreationTime descending
                                select new ProductViewModel
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Category = c.Name,
                                    Price = p.Price,
                                    Discount = p.Discount,
                                    BestSeller = (bool)p.BestSeller,
                                    ThumbnailImage = p.Image,
                                    DiscountPrice = p.Price * (100 - p.Discount) / 100,
                                    Url = p.Url
                                };
            ViewBag.DisplaySlider = false;
            var products = await productsQuery.ToPagedListAsync<ProductViewModel>((int)currentPage, 10);
            ViewBag.Title = "Gchill sản phẩm";
            ViewBag.Description = "Các sản phẩm của Gchill, hãy để chúng tôi phục vụ bạn";
            return View(products);
        }
    }
}
