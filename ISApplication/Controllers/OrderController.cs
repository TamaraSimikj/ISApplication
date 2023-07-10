using System.Globalization;
using System.Security.Claims;
using System.Text;
using Domain.DomainModels;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace ISApplication.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;

            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult Index()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var user = _userService.GetUser(userId);
            if(user != null && user.Role == Roles.Normal)
            {
                return RedirectToAction("NotPermission", "Home");
            }
            var orders= _orderService.GetAllOrders();
            return View(orders);
        }

        public IActionResult Details(Guid id)
        {
            var order = _orderService.GetOrder(id);
            return View(order);
        }
        public FileContentResult CreateInvoice(Guid id)
        {

            var order = _orderService.GetOrder(id);
            var faktura = Path.Combine(Directory.GetCurrentDirectory(), "Faktura.docx");
            var docx = DocumentModel.Load(faktura);
            docx.Content.Replace("{{OrderNumber}}", order?.Id.ToString() ?? string.Empty);
            docx.Content.Replace("{{UserName}}", order.User.UserName);
            var str = new StringBuilder();
            double sum = 0;
            foreach(var item in order.ProductInOrders)
            {
                str.AppendLine(item.Product.ProductName);
                sum = sum + (item.Product.ProductPrice * item.Quantity);
            }
            docx.Content.Replace("{{ProductList}}", str.ToString());
            docx.Content.Replace("{{TotalPrice}}", sum.ToString(CultureInfo.InvariantCulture) + "$");
            var stream = new MemoryStream();
            docx.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(),new PdfSaveOptions().ContentType,"Faktura.pdf");
        }

        public IActionResult Orders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.GetUser(userId);
            var ordersFromCurrentUser = _orderService.GetAllOrders().Where(z=>z.User != null && z.User.Equals(user));
            return View(ordersFromCurrentUser);
        }
    }
}
