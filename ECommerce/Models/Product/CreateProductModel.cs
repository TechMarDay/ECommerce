﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ECommerce.Models.Product
{
    public class CreateProductModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public List<IFormFile> ThumbnailImages { get; set; }

        public IFormFile ThumbnailImage { get; set; }

        public decimal Price { get; set; }

        public int? Discount { get; set; } = 0;

        public decimal? DiscountPrice { get; set; } = 0;

        public bool BestSeller { get; set; }

        public string Url { get; set; }
    }
}