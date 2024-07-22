using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MimeKit;
using Org.BouncyCastle.Asn1.Cmp;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace UrFU_WorkSpace_API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _codeExpiration = TimeSpan.FromMinutes(10);

    public UserService(IUserRepository userRepository, IConfiguration configuration, IMemoryCache memoryCache)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _memoryCache = memoryCache;
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.FindAll();
    }
    
    public Result<string> Authenticate(AuthenticateRequest authenticate)
    {

        var users = _userRepository.FindAll();
        var user = users.FirstOrDefault(x => x.Login == authenticate.Login && x.Password == authenticate.Password);
        
        return user == null ? 
            new Result<string>(ErrorHandler.RenderError(ErrorType.UserNotFound)) : 
            JwtTokenHelper.GenerateJwtToken( _configuration, user);
    }
    
    public Result<string> Register(User user)
    {
        var isUserExist = IsUserExist(user);
        if (!isUserExist)
        {
            return Result.Fail<string>(ErrorHandler.RenderError(ErrorType.UserConflict));
        }
        
        return JwtTokenHelper.GenerateJwtToken( _configuration, user);
    }

    public IQueryable<User> GetUsersByCondition(Expression<Func<User, bool>> expression)
    {
        return _userRepository.FindByCondition(expression);
    }

    private bool IsUserExist(User user)
    {
        var oldUser = GetUsersByCondition(u => u.Email == user.Email  || u.Login == user.Login).FirstOrDefault();
   
        return oldUser == null;
    }
    
    public Result<None> SendConfirmEmail(int code, string email)
    {
        var emailMessage = GetConfirmMessage(code, email);
        using var client = new SmtpClient();
        try
        {
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(_configuration["Email"], _configuration["Password"]);
            client.Send(emailMessage);
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail<None>(new Error(e.Message));
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
            emailMessage.Dispose();
        }
    }

    public void SaveUserInfo(User user, int code)
    {
        var codeKey = user.Login + " code";
        _memoryCache.Set(user.Login, user,  _codeExpiration);
        _memoryCache.Set(codeKey, code, _codeExpiration);
    }

    public Result<None> Confirm(string login, int code)
    {
        _memoryCache.TryGetValue(login, out User? user);
        _memoryCache.TryGetValue(login + " code", out int storedCode);
        if (user == null)
        {
            return Result.Fail<None>(ErrorHandler.RenderError(ErrorType.UserNotFound));
        }
        if (code != storedCode)
        {
            return Result.Fail<None>(ErrorHandler.RenderError(ErrorType.IncorrectConfirmCode));
        }
        
        user.AccessLevel = 1;
        var result = Result.Ok(user).Then(_userRepository.Create);
        _memoryCache.Remove(login);
        _memoryCache.Remove(login + " code");
        return result.IsSuccess ? Result.Ok() : Result.Fail<None>(result.Error);


    }
    
    private MimeMessage GetConfirmMessage(int code, string email)
    {
        const string name = "Администрация сайта";
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(name, _configuration["Email"]));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = name;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = code.ToString()
        };
        return emailMessage;
    }
}