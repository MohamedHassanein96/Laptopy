using LaptopyCore.IUnitOfWorkRepository;
using LaptopyCore.Model;
using LaptopyEF.UnitOfWorkRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace Laptopy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly UserManager<ApplicationUser> _userManger;

        public CartController(IUnitOfWorkRepository unitOfWorkRepository, UserManager<ApplicationUser> userManger)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
              _userManger = userManger;
        }



        [HttpPost("AddToCart")]

        public IActionResult AddToCart(int productId,int count)
        {

            var user = _userManger.GetUserId(User);
            Cart cart = new Cart()
            {
                Count = count,
                ProductId = productId,
                ApplicationUserId = user
            };
             var cartProduct= _unitOfWorkRepository.Carts.Get(e => e.ProductId == productId && e.ApplicationUserId == user).FirstOrDefault();
            if (cartProduct == null)
            {
                _unitOfWorkRepository.Carts.Create(cart);
            }

            else
            {
                cartProduct.Count +=count;
            }
            _unitOfWorkRepository.SaveChanges();
            return Ok();

        }

        [HttpGet("GetAll")]
        public IActionResult Index()
        {
            var user = _userManger.GetUserId(User);
            var cartProduct = _unitOfWorkRepository.Carts.Get(e => e.ApplicationUserId == user, query => query.Include(e => e.Product));
            var shoppingCart = new
            {
                Carts = cartProduct,
                TotalPrice = cartProduct.Sum(e => (double)(e.Product.Price * e.Count))
            };
            return Ok(shoppingCart);


        }
        [HttpPut("Increment")]
        public IActionResult Increment(int productId)
        {
            var ApplicationUserId = _userManger.GetUserId(User);

            var product = _unitOfWorkRepository.Carts.Get( e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId).FirstOrDefault();

            if (product != null)
            {
                product.Count++;
                _unitOfWorkRepository.SaveChanges();

                return Ok();
            }

            return NotFound();
        }
        [HttpPut("Decrement")]
        public IActionResult Decrement(int productId)
        {
            var ApplicationUserId = _userManger.GetUserId(User);

            var product = _unitOfWorkRepository.Carts.Get( e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId).FirstOrDefault();

            if (product != null)
            {
                product.Count--;

                if (product.Count == 0)
                {
                    _unitOfWorkRepository.Carts.Delete(product);
                }
                _unitOfWorkRepository.SaveChanges();

                return Ok();
            }

            return NotFound();
        }
        [HttpPut("Delete")]
        public IActionResult Delete(int productId)
        {
            var ApplicationUserId = _userManger.GetUserId(User);

            var product = _unitOfWorkRepository.Carts.Get( e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId).FirstOrDefault();

            if (product != null)
            {
                _unitOfWorkRepository.Carts.Delete(product);
                _unitOfWorkRepository.SaveChanges();

                return Ok();
            }

            return NotFound();
        }
        [HttpPost("Pay")]
        public IActionResult Pay()
        {
            var applicationUserId = _userManger.GetUserId(User);

            var cartProduct = _unitOfWorkRepository.Carts.Get(c=>c.ApplicationUserId==applicationUserId, query=>query.Include(e=>e.Product));

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            foreach (var item in cartProduct)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                        UnitAmount = (long)item.Product.Price * 100,
                    },
                    Quantity = item.Count,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Created(session.Url, cartProduct);
        }
    }
}
