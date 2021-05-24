using Microsoft.AspNetCore.Http;

namespace ECommerce.Models.Product
{
    public class EditProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public IFormFile ThumbnailImage { get; set; }

        public decimal Price { get; set; }

        public int? Discount { get; set; } = 0;

        public bool BestSeller { get; set; }

        public string Url { get; set; }
    }
}