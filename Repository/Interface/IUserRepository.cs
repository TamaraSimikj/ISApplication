using System.Collections.Generic;
using Domain.Identity;

namespace Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<CinemaApplicationUser?> GetAll();
        CinemaApplicationUser? Get(string id);
        void Insert(CinemaApplicationUser? entity);
        void Update(CinemaApplicationUser? entity);
        void Delete(CinemaApplicationUser? entity);

        public CinemaApplicationUser? GetUserByEmail(string email);
    }
}