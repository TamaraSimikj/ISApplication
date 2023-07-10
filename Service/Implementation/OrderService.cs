using Domain.DomainModels;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order?> _orderService;

        public OrderService(IRepository<Order?> orderService)
        {
            this._orderService = orderService;
        }

        public Order? GetOrder(Guid id)
        {
            return _orderService.Get(id);
        }

        public List<Order?> GetAllOrders()
        {
            return _orderService.GetAll().ToList();
        }
    }
}
