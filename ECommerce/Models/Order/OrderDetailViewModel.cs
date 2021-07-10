using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models.Order
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }


        public List<ProductOrderViewModel> productOrderViewModels { get; set; }
    }

    public class ProductOrderViewModel
    {
        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
