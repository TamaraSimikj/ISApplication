using Domain.DomainModels;

namespace Repository.Interface
{
    public interface IOrderRepository
    {
        public List<Order> getAllOrders();

        public Order? getOrder(Guid id);
    }
}