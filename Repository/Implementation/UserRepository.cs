
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        
        private readonly ApplicationDbContext _dbContext;
        private DbSet<CinemaApplicationUser?> users;
        
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            users = dbContext.Set<CinemaApplicationUser>();
        }
        

        public IEnumerable<CinemaApplicationUser?> GetAll()
        {
            return users.AsEnumerable();
        }

        public CinemaApplicationUser? Get(string id)
        {
            return users
                .Include(z => z.OwnersCard)
                .Include("OwnersCard.ProductInShoppingCard")
                .Include("OwnersCard.ProductInShoppingCard.CurrentProduct")
                .SingleOrDefault(s => s.Id.Equals(id));
        }
        public void Insert(CinemaApplicationUser? entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            users.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Update(CinemaApplicationUser? entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            users.Update(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(CinemaApplicationUser? entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            users.Remove(entity);
            _dbContext.SaveChanges();
        }

        public CinemaApplicationUser? GetUserByEmail(string email)
        {
            return users.SingleOrDefault(s => s.Email.Equals(email));
        }
    }
    
}