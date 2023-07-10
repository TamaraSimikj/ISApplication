using System.Globalization;
using System.Security.Claims;
using ClosedXML.Excel;
using Domain.DomainModels;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interface;

namespace ISApplication.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        private readonly IUserService _userService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService, IUserService userService)
        {
            _logger = logger;
            _productService = productService;
            this._userService = userService;
        }

        // GET: Products
        public IActionResult Index()
        {
            _logger.LogInformation("User Request -> Get All products!");
            return View(_productService.GetAllProducts());
        }

        // GET: Products/Details/5
        public IActionResult Details(Guid? id)
        {
            _logger.LogInformation("User Request -> Get Details For Product");
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetDetailsForProduct(id);
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.GetUser(userId);

            return View();

        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ProductName,ProductImage,ProductDescription,ProductPrice,ProductRating")] Product product)
        {
            _logger.LogInformation("User Request -> Inser Product in DataBase!");
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                _productService.CreateNewProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        

        // GET: Products/Edit/5
        public IActionResult Edit(Guid? id)
        {
            _logger.LogInformation("User Request -> Get edit form for Product!");
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(id);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,ProductName,ProductImage,ProductDescription,ProductPrice,ProductRating")] Product product)
        {
            _logger.LogInformation("User Request -> Update Product in DataBase!");

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._productService.UpdateExistingProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(Guid? id)
        {
            _logger.LogInformation("User Request -> Get delete form for Product!");

            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(id);

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("User Request -> Delete Product in DataBase!");

            this._productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult ExcelView()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.GetUser(userId);
            if (user != null && user.Role.Equals(Roles.Admin))
            {
                return View();
            }
            return RedirectToAction("NotPermission", "Home");
            
        }

        [HttpPost]
        public IActionResult ExcelView(Product product)
        {
            var genre = product.ProductGenre;
            var productsOfGenre = _productService.GetAllProducts().Where(z => z.ProductGenre.Equals(genre)).ToList();
            var fileName = $"Genre {genre.ToString()}.xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Tickets");
            worksheet.Cell(1, 1).Value = "Ticket Id";
            worksheet.Cell(1, 2).Value = "Ticket title";
            worksheet.Cell(1, 3).Value = "Ticket price";
            worksheet.Cell(1, 4).Value = "Valid until";
            worksheet.Cell(1, 5).Value = "Genre";

            for (var i = 1; i <= productsOfGenre.Count(); i++)
            {
                worksheet.Cell(i + 1, 1).Value = productsOfGenre[i - 1].Id.ToString();
                worksheet.Cell(i + 1, 2).Value = productsOfGenre[i - 1].ProductName.ToString();
                worksheet.Cell(i + 1, 3).Value = $"{productsOfGenre[i - 1].ProductPrice.ToString(CultureInfo.InvariantCulture)}$";
                worksheet.Cell(i + 1, 4).Value = productsOfGenre[i - 1].ExpirationDateTime.ToString(CultureInfo.InvariantCulture);
                worksheet.Cell(i + 1, 5).Value = productsOfGenre[i - 1].ProductGenre.ToString();
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, contentType, fileName);
        }


        public IActionResult Add(Guid id)
        {
            var result = this._productService.GetShoppingCartInfo(id);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddToShoppingCardDTO model)
        {

            _logger.LogInformation("User Request -> Add Product in ShoppingCart and save changes in database!");


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._productService.AddToShoppingCart(model, userId);

            if(result)
            {
                return RedirectToAction("Index", "Products");
            }
            return View(model);
        }
        private bool ProductExists(Guid id)
        {
            return _productService.GetDetailsForProduct(id) != null;
        }
    }
}
