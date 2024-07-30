using MailKit.Net.Smtp;
using Microsoft.Extensions.Caching.Memory;
using MimeKit;
using MimeKit.Text;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services;

public class AuthenticationService
{
    private readonly TimeSpan _codeExpiration = TimeSpan.FromMinutes(10);
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;
    private readonly ErrorHandler _errorHandler;
    private readonly IUserService _userService;

    public AuthenticationService(IUserService userService, IConfiguration configuration, IMemoryCache memoryCache, ErrorHandler errorHandler)
    {
        _configuration = configuration;
        _memoryCache = memoryCache;
        _errorHandler = errorHandler;
        _userService = userService;
    }


    public Result<string> GetAccessToken(TokenRequest model)
    {
        Result<string> result;
        switch (model.GrantType)
        {
            case GrantType.Code:
                result = Authenticate(model.Code);
                break;
            case GrantType.Password:
                result = Authenticate(model.Login, model.Password);
                break;
            default:
                return Result.Fail<string>(_errorHandler.RenderError(ErrorType.InvalidGrantType));
        }

        return result;
    }

    public Result<int> Register(User user)
    {
        var isUserExist = _userService.GetUsersByCondition(u => u.Email == user.Email || u.Login == user.Login)
            .FirstOrDefault();
        if (isUserExist != null) return Result.Fail<int>(_errorHandler.RenderError(ErrorType.UserConflict));
        user.AccessLevel = (int)Role.User;
        var code = Utils.GetConfirmCode();
        _memoryCache.Set(code, user);
        return code;
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

    private MimeMessage GetConfirmMessage(int code, string email)
    {
        const string name = "Администрация сайта";
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(name, _configuration["Email"]));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = name;
        emailMessage.Body = new TextPart(TextFormat.Html)
        {
            Text = code.ToString()
        };
        return emailMessage;
    }

    private Result<string> Authenticate(string? login, string? password)
    {
        if (login == null || password == null)
            return Result.Fail<string>(_errorHandler.RenderError(ErrorType.BadAuthRequest));
        var user = _userService.GetUsersByCondition(x => x.Login == login && x.Password == password).FirstOrDefault();
        return GenerateUserJwtToken(user);
    }

    private Result<string> Authenticate(int code)
    {
        _memoryCache.TryGetValue(code, out User? user);
        if (user == null) return Result.Fail<string>(_errorHandler.RenderError(ErrorType.IncorrectConfirmCode));
        var result = Result.Ok(user).Then(_userService.CreateUser).Then(id =>
        {
            user.Id = id;
            return GenerateUserJwtToken(user);
        });
        _memoryCache.Remove(code);
        return result;
    }

    private Result<string> GenerateUserJwtToken(User? user)
    {
        return user == null
            ? new Result<string>(_errorHandler.RenderError(ErrorType.UserNotFound))
            : JwtTokenHelper.GenerateJwtToken(_configuration, user);
    }
}