using System.ComponentModel;
using Domain.Identity;
using Domain.Relations;

namespace Domain.DomainModels
{
    public sealed class Order :BaseEntity
    {
        public Order()
        {
        }

        [DisplayName("OrderId")]
        public Guid OrderId { get; set; }
        
        [DisplayName("User_order")]
        public CinemaApplicationUser? User { get; set; }
        
        [DisplayName("ProductsInOrder")]
        public ICollection<ProductInOrder> ProductInOrders { get; set; }
    }
}