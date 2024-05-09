using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Repository;

namespace UrFU_WorkSpace_API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    
    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest authenticate)
    {
        var user = _userRepository
            .GetAllUsers()
            .FirstOrDefault(x => (x.Login == authenticate.Login) && x.Password == authenticate.Password);
        if (user == null)
        {
            return new AuthenticateResponse("");
        }
        var token = _configuration.GenerateJwtToken(user);
        return new AuthenticateResponse(token);
    }

    public async Task<AuthenticateResponse> Register(User user)
    {
        _userRepository.AddUser(user);

        var response = Authenticate(new AuthenticateRequest
        {
            Login = user.Login,
            Password = user.Password
        });
        return response;
    }
    
    public bool IsUserExists(UserCheckRequest user)
    {
        return  _userRepository.GetAllUsers()
            .Any(u => u.Email.Trim().ToUpper() == user.Email.TrimEnd().ToUpper() 
                      || u.Login.Trim().ToUpper() == user.Login.TrimEnd().ToUpper());
    }
    
    
}