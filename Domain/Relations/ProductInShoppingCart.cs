using Domain.DomainModels;

namespace Domain.Relations
{
    public class ProductInShoppingCart : BaseEntity
    {
        public Guid ProductId { get; set; }
        
        public Product CurrentProduct { get; set; }

        public Guid ShoppingCartId { get; set; }
        
        public ShoppingCart OwnersCard { get; set; }

        public int Quantity { get; set; }
    }
}