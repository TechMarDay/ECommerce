using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models.Order
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public string Phone { get; set; }
    }
}
