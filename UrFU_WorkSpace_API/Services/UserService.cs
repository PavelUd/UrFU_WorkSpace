using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
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

    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.FindAll();
    }
    
    public AuthenticateResponse Authenticate(AuthenticateRequest authenticate)
    {

        var users = _userRepository.FindAll();
        var user = users.FirstOrDefault(x => (x.Login == authenticate.Login) && x.Password == authenticate.Password);
        
        return user == null ? 
            new AuthenticateResponse(HttpStatusCode.Unauthorized,message: "Пользователь не найден") : 
            new AuthenticateResponse(HttpStatusCode.OK,token: _configuration.GenerateJwtToken(user));
    }
    
    public async Task<AuthenticateResponse> Register(User user)
    {
       var statusCode = AddUser(user);
       if (statusCode != HttpStatusCode.Created)
       {
           var message = statusCode == HttpStatusCode.Conflict
               ? "Такой пользователь уже существует"
               : "пользователь не сохранился";
           return new AuthenticateResponse(statusCode, message);
       }
       return  Authenticate(new AuthenticateRequest
       {
           Login = user.Login,
           Password = user.Password
       });
    }

    public IEnumerable<User> GetUsersByCondition(Expression<Func<User, bool>> expression)
    {
        return _userRepository.FindByCondition(expression).AsEnumerable();
    }

    public void UpdateAccessLevel(int idUser, int newAccessLevel)
    {
        _userRepository.FindByCondition(x => x.Id == idUser)
            .ExecuteUpdate(b => b.SetProperty(x => x.AccessLevel, newAccessLevel));

    }

    private HttpStatusCode AddUser(User user)
    {
        user.AccessLevel = 0;
        var exitingUser = GetUsersByCondition(u => u.Email.Trim().ToUpper() == user.Email.TrimEnd().ToUpper() 
                                                  || u.Login.Trim().ToUpper() == user.Login.TrimEnd().ToUpper()).FirstOrDefault();

        if (exitingUser == null)
        {
            _userRepository.Create(user);
        }
        else if (exitingUser.AccessLevel == 0)
        {
            user.Id = exitingUser.Id;
            _userRepository.Update(user);
        }
        else
        {
            return HttpStatusCode.Conflict;
        }
        return _userRepository.Save() ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
    }
}