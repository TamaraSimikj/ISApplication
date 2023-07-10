using System.ComponentModel;
using Domain.Identity;
using Domain.Relations;

namespace Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        [DisplayName("OwnerCardId")]
        public string OwnerId { get; set; }
        
        [DisplayName("OwnerCard")]
        public virtual CinemaApplicationUser Owner { get; set; }

        [DisplayName("ProductsInCard")]
        public virtual ICollection<ProductInShoppingCart?> ProductInShoppingCard { get; set; }

    }
}