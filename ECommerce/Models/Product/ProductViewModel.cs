using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Category { get; set; }

        public string ThumbnailImage { get; set; }

        public decimal Price { get; set; }

        public int? Discount { get; set; } = 0;

        public decimal? DiscountPrice { get; set; } = 0;

        public bool BestSeller { get; set; }
    }
}
