using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public interface IStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string filePath);

        Task DeleteFileAsync(string filePath);
    }
}