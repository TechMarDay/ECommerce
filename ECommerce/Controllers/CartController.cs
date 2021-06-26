using ECommerce.Constants;
using ECommerce.Data;
using ECommerce.Models.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public IActionResult Checkout(CheckoutViewModel request)
        {
            var checkoutVm = GetCheckoutViewModel();
            var oderDeatails = new List<OderDetailVm>();
            foreach(var item in checkoutVm.CartItems)
            {
                oderDeatails.Add(new OderDetailVm
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            var checkoutRequest = new CheckoutRequest()
            {
                Address = request.CheckoutModel.Address,
                Name = request.CheckoutModel.Name,
                Email = request.CheckoutModel.Email,
                PhoneNumber = request.CheckoutModel.PhoneNumber,
                OrderDetails = oderDeatails
            };

            //Todo: Add data into database + send mail

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