using Domain.DomainModels;
using Domain.DTO;
using Domain.Relations;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation
{
    public class ProductService : IProductService
    {

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductInShoppingCart> _productInShoppingCartRepository;
        private readonly IUserRepository _userRepository;

        public ProductService(IRepository<Product> productRepository, IRepository<ProductInShoppingCart> productInShoppingCartRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
            _userRepository = userRepository;
        }

        public void CreateNewProduct(Product p)
        {
            _productRepository.Insert(p);
        }

        public void UpdateExistingProduct(Product p)
        {
            _productRepository.Update(p);
        }

        public void DeleteProduct(Guid id)
        {
            var product = GetDetailsForProduct(id);
            _productRepository.Delete(product);
        }

        public bool AddToShoppingCart(AddToShoppingCardDTO item, string userId)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAll().ToList();
        }

        public Product GetDetailsForProduct(Guid? id)
        {
            return this._productRepository.Get(id);
        }

        public AddToShoppingCardDTO GetShoppingCartInfo(Guid? id)
        {
            var product = this.GetDetailsForProduct(id);
            AddToShoppingCardDTO model = new AddToShoppingCardDTO
            {
                SelectedProduct = product,
                SelectedProductId = product.Id,
                Quantity = 1
            };

            return model;
        }

    }
}