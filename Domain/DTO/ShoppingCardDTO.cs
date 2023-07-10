using System.ComponentModel;
using Domain.Relations;

namespace Domain.DTO
{
    public class ShoppingCardDTO
    {
        [DisplayName("Products")]
        public List<ProductInShoppingCart?> Products { get; set; }

        [DisplayName("TotalPrice")]
        public double TotalPrice { get; set; }
    }
}