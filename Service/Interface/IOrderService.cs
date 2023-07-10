using Domain.DomainModels;

namespace Service.Interface
{
    public interface IOrderService
    {
        public Order? GetOrder(Guid id);
        public List<Order?> GetAllOrders();
    }
}