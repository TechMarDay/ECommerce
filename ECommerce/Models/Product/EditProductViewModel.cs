using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models.Product
{
    public class EditProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProductCategoryModel Category { get; set; }

        public string ThumbnailImage { get; set; }

        public decimal Price { get; set; }

        public int? Discount { get; set; } = 0;

        public bool BestSeller { get; set; }
    }
}
