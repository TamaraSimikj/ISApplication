using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Stripe;

namespace ISApplication.Controllers
{
    public class ShoppingCardController : Controller
    {
        private readonly ICardService _shoppingCartService;

        public ShoppingCardController(ICardService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(this._shoppingCartService.GetCardInfo(userId));
        }

        public IActionResult DeleteFromShoppingCart(Guid id)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.DeleteProductFromCard(userId, id);
            
            if(result)
            {
                return RedirectToAction("Index", "ShoppingCard");
            }
            else
            {
                return RedirectToAction("Index", "ShoppingCard");
            }
        }

        public Boolean Order()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.OrderProductsAsync(userId.ToString());

            return result;
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = this._shoppingCartService.GetCardInfo(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source =stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "Gorjan's Cinema",
                Currency = "MKD",
                Customer = customer.Id
            });

            if (charge.Status != "succeeded") return RedirectToAction("Index", "ShoppingCard");
            var result = this.Order();

            return RedirectToAction("Index", "ShoppingCard");

        }
    }
}