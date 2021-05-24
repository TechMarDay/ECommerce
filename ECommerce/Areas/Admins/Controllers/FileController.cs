using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace ECommerce.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class FileController : Controller
    {
        private IWebHostEnvironment hostingEnvironment;

        public FileController(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        [Route("uploadCkeditor")]
        [HttpPost]
        public IActionResult Upload(IFormFile upload)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "uploads", fileName);
            var stream = new FileStream(path, FileMode.Create);
            upload.CopyTo(stream);
            return new JsonResult(new

            {
                uploaded = 1,
                fileName = upload.FileName,
                url = "/uploads/" + fileName
            });
        }

        [Route("uploadProductCkeditor")]
        [HttpPost]
        public IActionResult UploadProduct(IFormFile upload)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "uploads/products", fileName);
            var stream = new FileStream(path, FileMode.Create);
            upload.CopyTo(stream);
            return new JsonResult(new

            {
                uploaded = 1,
                fileName = upload.FileName,
                url = "uploads/products/" + fileName
            });
        }

        [Route("filebrowse")]
        [HttpGet]
        public IActionResult Filebrowse()
        {
            var dir = new DirectoryInfo(Path.Combine(
                Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "uploads"));
            ViewBag.fileInfos = dir.GetFiles();
            return View("FileBrowse");
        }
    }
}