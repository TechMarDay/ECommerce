using ECommerce.Constants;
using ECommerce.Data;
using ECommerce.Entities;
using ECommerce.Extension;
using ECommerce.Models.Product;
using ECommerce.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Route("admins/product")]
    public class ProductController : Controller
    {
        private readonly EcommerceDbContext dbContext;
        private readonly IStorageService storageService;


        public ProductController(EcommerceDbContext dbContext,
            IStorageService storageService)
        {
            this.dbContext = dbContext;
            this.storageService = storageService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(int? currentPage)
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
                                    ThumbnailImage = p.Image
                                };

            var products = await productsQuery.ToPagedListAsync<ProductViewModel>((int)currentPage, 5);
            return View(products);
        }

        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.Categories = await dbContext.ProductCategories.Select(x => new ProductCategoryModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return View();
        }

        [HttpPost]
        [Route("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProductModel model)
        {
            //Validate
            var hasError = false;
            if (model.ThumbnailImage == null)
            {
                hasError = true;
                ViewBag.Error += "Vui lòng chọn hình ảnh";
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                hasError = true;
                ViewBag.Error += ", nhập tên";
            }

            if (model.Price <= 0)
            {
                hasError = true;
                ViewBag.Error += ", nhập giá";
            }

            if (string.IsNullOrWhiteSpace(model.Description))
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
                ViewBag.Categories = await dbContext.ProductCategories.Select(x => new ProductCategoryModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

                return View(model);
            }

            if (ModelState.IsValid)
            {
                var product = new ProductEntity
                {
                    Name = model.Name,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    Price = model.Price,
                    Discount = model.Discount,
                    DiscountPrice = model.DiscountPrice,
                    BestSeller = model.BestSeller,
                    CreationTime = DateTime.UtcNow,
                    Url = model.Url
                };

                if (model.ThumbnailImage != null)
                {
                    var thumbnailImageUrl = await storageService.SaveFileAsync(model.ThumbnailImage,
                        UploadPathConstant.ProductPath);
                    product.Image = thumbnailImageUrl;
                }

                dbContext.Add(product);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = await dbContext.ProductCategories.Select(x => new ProductCategoryModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

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

            var productQuery = from p in dbContext.Products
                               join c in dbContext.ProductCategories
                                    on p.CategoryId equals c.Id
                               select new EditProductViewModel
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   Description = p.Description,
                                   Price = p.Price,
                                   Discount = p.Discount,
                                   BestSeller = (bool)p.BestSeller,
                                   ThumbnailImage = p.Image,
                                   Url = p.Url,
                                   Category = new ProductCategoryModel
                                   {
                                       Id = c.Id,
                                       Name = c.Name
                                   }
                               };

            var productModel = await productQuery.Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (productModel == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await dbContext.ProductCategories.Select(x => new ProductCategoryModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return View(productModel);
        }

        [Route("edit")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EditAsync(EditProductModel productModel)
        {
            //Validate
            var hasError = false;

            if (string.IsNullOrWhiteSpace(productModel.Name))
                hasError = true;

            if (productModel.Price <= 0)
                hasError = true;

            if (string.IsNullOrWhiteSpace(productModel.Description))
                hasError = true;

            if (string.IsNullOrWhiteSpace(productModel.Url))
                hasError = true;

            if (hasError)
            {
                ViewBag.Categories = await dbContext.ProductCategories.Select(x => new ProductCategoryModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

                return RedirectToAction("edit", new { id = productModel.Id });
            }

            var product = await dbContext.Products.Where(x => x.Id == productModel.Id)
                    .FirstOrDefaultAsync();

            product.Name = productModel.Name;
            product.Description = productModel.Description;
            product.Discount = productModel.Discount;
            product.Price = productModel.Price;
            product.BestSeller = productModel.BestSeller;
            product.CategoryId = productModel.CategoryId;
            product.LastModificationTime = DateTime.UtcNow;
            product.Url = productModel.Url;

            if (productModel.ThumbnailImage != null)
            {
                var thumbnailImageUrl = await storageService.SaveFileAsync(productModel.ThumbnailImage,
                    UploadPathConstant.ProductPath);
                product.Image = thumbnailImageUrl;
            }

            dbContext.Update(product);

            await dbContext.SaveChangesAsync();
            return RedirectToAction("edit", new { id = productModel.Id });
        }


        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            var product = await dbContext.Products.FindAsync(id);
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}