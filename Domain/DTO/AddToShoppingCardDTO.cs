using System.ComponentModel;
using Domain.DomainModels;

namespace Domain.DTO
{
    public class AddToShoppingCardDTO
    {
        [DisplayName("SelectedProduct")]
        public Product SelectedProduct { get; set; }
        
        [DisplayName("SelectedProductID")]
        public Guid SelectedProductId { get; set; }
        
        [DisplayName("Quantity")]
        public int Quantity { get; set; }
    }
}