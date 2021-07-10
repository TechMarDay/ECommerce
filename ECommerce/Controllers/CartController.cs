using ECommerce.Constants;
using ECommerce.Data;
using ECommerce.Entities;
using ECommerce.Models.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    [Route("giohang")]
    public class CartController : Controller
    {
        private readonly EcommerceDbContext dbContext;

        public CartController(EcommerceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.DisplaySlider = false;
            ViewBag.Title = "Gchill giỏ hàng";
            return View();
        }

        [HttpGet("items")]
        public IActionResult GetListItems()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            return Ok(currentCart);
        }

        [HttpPost]
        //[Route("addToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddCartItemViewModel model)
        {
            var product = await dbContext.Products.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            int quantity = 1;
            var cartItem = new CartItemViewModel();
            if (currentCart.Any(x => x.ProductId == model.Id))
            {
                var currentCartItem = currentCart.FirstOrDefault(x => x.ProductId == model.Id);
                currentCartItem.Quantity += 1;
            }
            else
            {
                cartItem = new CartItemViewModel
                {
                    ProductId = model.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Image = product.Image,
                    Price = product.Price,
                    Quantity = quantity
                };
                currentCart.Add(cartItem);
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        [HttpPut]
        public IActionResult UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);

            foreach (var item in currentCart)
            {
                if (item.ProductId == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }
                    item.Quantity = quantity;
                }
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        [HttpGet]
        [Route("dathang")]
        public IActionResult Checkout()
        {
            var checkoutVm = GetCheckoutViewModel();
            return View(checkoutVm);
        }

        [HttpPost]
        [Route("dathang")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CheckoutAsync([FromForm] CheckoutViewModel request)
        {
            var checkoutVm = GetCheckoutViewModel();
            //var oderDeatails = new List<OderDetailVm>();
            var orderDetailEntities = new List<OrderDetailEntity>();
            //foreach(var item in checkoutVm.CartItems)
            //{
            //    oderDeatails.Add(new OderDetailVm
            //    {
            //        ProductId = item.ProductId,
            //        Quantity = item.Quantity,
            //        Price = item.Price
            //    });
            //}

            var checkoutRequest = new CheckoutRequest()
            {
                Address = request.Address,
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                //OrderDetails = oderDeatails
            };

            //Todo: Add data into database + send mail
            var order = new OrderEntity
            {
                OrderDate = DateTime.UtcNow,
                Status = Models.OrderStatus.InProgress,
                ShipAddress = request.Address,
                ShipPhoneNumber = request.PhoneNumber,
                ShipEmail = request.Email,
                ShipName = request.Name,
                CreationTime = DateTime.UtcNow
            };

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            foreach (var item in checkoutVm.CartItems)
            {
                var detail = new OrderDetailEntity
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    OrderId = order.Id
                };
                await dbContext.AddAsync(detail);
            }
            await dbContext.SaveChangesAsync();

            TempData["SuccessMsg"] = "Chúc mừng bạn. Đơn hàng của bạn đã được đặt thành công Bộ phận chăm sóc khách hàng sẽ liên lạc trong vòng 2h để xác nhận Chi tiết đơn hàng.";
            return View(checkoutVm);
        }

        private CheckoutViewModel GetCheckoutViewModel()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            var checkoutVm = new CheckoutViewModel()
            {
                CartItems = currentCart,
                CheckoutModel = new CheckoutRequest()
            };
            return checkoutVm;
        }
    }
}