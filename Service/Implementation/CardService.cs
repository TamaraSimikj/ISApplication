using System.Globalization;
using System.Text;
using Domain.DomainModels;
using Domain.DTO;
using Domain.Relations;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

    public class CardService : ICardService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<EmailMessage> _mailRepository;
        private readonly IRepository<ProductInOrder> _productInOrderRepository;
        private readonly IUserRepository _userRepository;

        public CardService(IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<EmailMessage> mailRepository, IRepository<Order> orderRepository, IRepository<ProductInOrder> productInOrderRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _productInOrderRepository = productInOrderRepository;
            _mailRepository = mailRepository;
        }


        public bool DeleteProductFromCard(string userId, Guid? productId)
        {
            if(!string.IsNullOrEmpty(userId) && productId != null)
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.OwnersCard;

                var itemToDelete = userShoppingCart.ProductInShoppingCard.Where(z => z.ProductId.Equals(productId)).FirstOrDefault();

                userShoppingCart.ProductInShoppingCard.Remove(itemToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }
            return false;
        }

        public ShoppingCardDTO GetCardInfo(string userId)
        {
            if(!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userCard = loggedInUser.OwnersCard;

                var allProducts = userCard.ProductInShoppingCard.ToList();

                var allProductPrices = allProducts.Select(z => new
                {
                    ProductPrice = z.CurrentProduct.ProductPrice,
                    Quantity = z.Quantity
                }).ToList();

                double totalPrice = 0.0;

                foreach (var item in allProductPrices)
                {
                    totalPrice += item.Quantity * item.ProductPrice;
                }

                var reuslt = new ShoppingCardDTO
                {
                    Products = allProducts,
                    TotalPrice = totalPrice
                };

                return reuslt;
            }
            return new ShoppingCardDTO();
        }
        

        public bool OrderProductsAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = _userRepository.Get(userId);
                var userCard = loggedInUser?.OwnersCard;

               
                Order order = new Order();
                order.User = loggedInUser;
                order.OrderId = Guid.Parse(userId);
                
                this._orderRepository.Insert(order);

                List<ProductInOrder> productInOrders = new List<ProductInOrder>();

                var result = userCard?.ProductInShoppingCard.Select(z => new ProductInOrder
                {
                    Id = Guid.NewGuid(),
                    ProductId = z.CurrentProduct.Id,
                    Product = z.CurrentProduct,
                    OrderId = order.Id,
                    Order = order, 
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0.0;

                sb.AppendLine("Your order is completed. The order conatins: ");

                if (result != null)
                    for (int i = 1; i <= result.Count(); i++)
                    {
                        var currentItem = result[i - 1];
                        totalPrice += currentItem.Quantity * currentItem.Product.ProductPrice;
                        sb.AppendLine(i + ". " + currentItem.Product.ProductName + " with quantity of: " +
                                      currentItem.Quantity + " and price of: $" + currentItem.Product.ProductPrice);
                    }

                sb.AppendLine("Total price for your order: " + totalPrice.ToString(CultureInfo.InvariantCulture));

            
                string subject = "Sucessfuly created order!";
                string content = sb.ToString();

                EmailMessage mail = new EmailMessage(loggedInUser?.Email,subject,content,false);
                if (result != null) productInOrders.AddRange(result);

                foreach (var item in productInOrders)
                {
                    this._productInOrderRepository.Insert(item);
                }

                loggedInUser?.OwnersCard.ProductInShoppingCard.Clear();

                this._userRepository.Update(loggedInUser);
                this._mailRepository.Insert(mail);

                return true;
            }

            return false;
        }
    }