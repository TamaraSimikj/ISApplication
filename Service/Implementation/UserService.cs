using Domain.Identity;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public CinemaApplicationUser? GetUser(string id)
    {
        var user = _userRepository.Get(id);
        return user;
    }

    public List<CinemaApplicationUser?> GetAllUsers()
    {
        return _userRepository.GetAll().ToList();
    }

    public bool Update(CinemaApplicationUser? user)
    {
        if(user != null)
        {
            _userRepository.Update(user);
            return true;
        }
        return false;
    }

    public CinemaApplicationUser? GetUserByEmail(string email)
    {
        return _userRepository.GetUserByEmail(email);
    }

}