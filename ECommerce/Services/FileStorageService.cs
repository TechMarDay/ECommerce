using ECommerce.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class FileStorageService : IStorageService
    {
        private IWebHostEnvironment hostingEnvironment;

        public FileStorageService(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task DeleteFileAsync(string fileName, string filePath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, filePath, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string filePath)
        {
            var fileNameUnique = DateTime.Now.ToString("yyyyMMddHHmmss") + file.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, filePath, fileNameUnique);
            var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return filePath + fileNameUnique;
        }
    }
}
