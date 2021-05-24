using Microsoft.AspNetCore.Http;

namespace ECommerce.Models.News
{
    public class CreateNewsModel
    {
        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public IFormFile Image { get; set; }

        public string Url { get; set; }
    }
}