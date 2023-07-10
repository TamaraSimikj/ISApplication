using System.ComponentModel.DataAnnotations;
using Domain.Relations;

namespace Domain.DomainModels
{
    public class Product : BaseEntity
    {
        [Required]
        public string ProductName { get; set; }
        
        [Required]
        [EnumDataType(typeof(Genre))]
        public Genre ProductGenre { get; set; }
        
        [Required]
        public string ProductImage { get; set; }
        
        [Required]
        public string ProductDescription { get; set; }
        
        [Required]
        public double ProductPrice { get; set; }
        
        [Required]
        public double ProductRating { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        public virtual ICollection<ProductInOrder> ProductInOrders { get; set; }
        
    }
}