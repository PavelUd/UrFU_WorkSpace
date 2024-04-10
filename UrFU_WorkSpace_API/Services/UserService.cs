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
            .FirstOrDefault(x => (x.Login == authenticate.Login || x.Email == authenticate.Email) && x.Password == authenticate.Password);
        
        var token = _configuration.GenerateJwtToken(user);
        
        return new AuthenticateResponse(user, token);
    }

    public async Task<AuthenticateResponse> Register(User user)
    {
        _userRepository.AddUser(user);

        var response = Authenticate(new AuthenticateRequest
        {
            Login = user.Login,
            Email = user.Email,
            Password = user.Password
        });
            
        return response;
    }
    
    public bool IsUserExists(User user)
    {
        return  _userRepository.GetAllUsers()
            .Any(u => u.Email.Trim().ToUpper() == user.Email.TrimEnd().ToUpper() 
                      || u.Login.Trim().ToUpper() == user.Login.TrimEnd().ToUpper());
    }
}