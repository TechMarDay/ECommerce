using ECommerce.Data;
using ECommerce.Extension;
using ECommerce.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Route("admins/order")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly EcommerceDbContext dbContext;

        public OrderController(EcommerceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(int? currentPage)
        {
            if (currentPage == null || currentPage == 0)
                currentPage = 1;

            var ordersQuery = from o in dbContext.Orders
                              orderby o.OrderDate descending
                              select new OrderViewModel
                              {
                                  Id = o.Id,
                                  CustomerName = o.ShipName,
                                  Address = o.ShipAddress,
                                  OrderDate = o.OrderDate,
                                  Phone = o.ShipPhoneNumber,
                                  Status = o.Status.ToString()
                              };

            var orders = await ordersQuery.ToPagedListAsync<OrderViewModel>((int)currentPage, 5);
            return View(orders);
        }

        [HttpGet]
        [Route("detail")]
        public async Task<IActionResult> DetailAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetailQuery = from o in dbContext.Orders
                                   where o.Id == id
                                   select new OrderDetailViewModel
                                   {
                                       Id = o.Id,
                                       Address = o.ShipAddress,
                                       CustomerName = o.ShipName,
                                       Phone = o.ShipPhoneNumber,
                                       Email = o.ShipEmail,
                                       OrderDate = o.OrderDate,
                                       Status = o.Status.ToString()
                                   };

            var orderDetail = await orderDetailQuery.FirstOrDefaultAsync();
            if (orderDetail == null)
            {
                return NotFound();
            }

            var orderDetailProduct = from od in dbContext.OrderDetails
                                     where od.Id == id
                                     join p in dbContext.Products
                                     on od.ProductId equals p.Id
                                     select new ProductOrderViewModel
                                     {
                                         ProductName = p.Name,
                                         ProductImage = p.Image,
                                         Price = p.Price,
                                         Quantity = od.Quantity
                                     };
            var orders = await orderDetailProduct.ToListAsync();
            orderDetail.productOrderViewModels = orders;
            return View(orderDetail);
        }

    }
}
