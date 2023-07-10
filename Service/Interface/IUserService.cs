using Domain.Identity;

namespace Service.Interface;

public interface IUserService
{
    public CinemaApplicationUser? GetUser(string id);

    public List<CinemaApplicationUser?> GetAllUsers();

    public CinemaApplicationUser? GetUserByEmail(string email);
    
    public bool Update(CinemaApplicationUser user);

}