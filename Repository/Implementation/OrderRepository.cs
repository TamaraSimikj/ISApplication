using Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        readonly DbSet<Order> _orders;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _orders = dbContext.Set<Order>();
        }
        
        public Order? getOrder(Guid id)
        {
            return _orders
                .Include(z => z.ProductInOrders)
                .Include("ProductInOrders.Product")
                .Include(z=>z.User)
                .SingleOrDefaultAsync(z=>z.Id == id)
                .Result;
        }
        
        public List<Order> getAllOrders()
        {
            return _orders
                .Include(z=> z.ProductInOrders)
                .Include("ProductInOrders.Product")
                .Include(z=>z.User)
                .ToListAsync().Result;
        }
        
    }
}