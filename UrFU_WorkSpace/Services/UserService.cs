using System.Net;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class UserClient : IUserService
{
    private IUserRepository _userRepository;
    private string Pattern = @"^[a-zA-Z0-9._%+-]+@urfu\.me$";
    
    public UserClient(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthenticateResponse> Register(IFormCollection form)
    {
        var dictionary = new Dictionary<string, object>()
        {
            { "firstName", form["first-name"].ToString() },
            { "lastName", form["second-name"].ToString() },
            { "email", form["email"].ToString() },
            { "login", form["login"].ToString() },
            { "password", form["password"].ToString() }
        };
        var response = await _userRepository.Register(dictionary);
        return response;
    }
    
    public async Task<AuthenticateResponse> Login(IFormCollection form)
    {
        var dictionary = new Dictionary<string, object>()
        {
            { "login", form["login"].ToString() },
            { "password", form["password"].ToString() }
        };
        return await _userRepository.Login(dictionary);

    }
    
    public async Task<string> SendEmailAsync(string email, string subject)
    {
        using var emailMessage = new MimeMessage();
        var random = new Random();
        var code = random.Next(100000, 1000000).ToString();
        emailMessage.From.Add(new MailboxAddress("Администрация сайта", "kovorkingiurfu@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = code
        };

        using var client = new SmtpClient();
        try
        {
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate("kovorkingiurfu@gmail.com", "iryhnjxlwvtgabsw");
            client.Send(emailMessage);
            return code;
        }
        catch (Exception e)
        {
            return e.Message; 
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }
    }

    public AuthenticateResponse VerifyUser(int idUser, string userCode, string verificationCode)
    {
        if (verificationCode != userCode)
        {
            return new AuthenticateResponse()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Неверный Код"
            };
        }
        
        var isSaved = UpdateAccessLevel(idUser, 1).Result;
        if (!isSaved)
        {
            return new AuthenticateResponse()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Статутс не обновлен"
            };
        }
        return new AuthenticateResponse()
        {
            StatusCode = HttpStatusCode.OK
        };
    }
    
    public async Task<bool> UpdateAccessLevel(int idUser, int newAccessLevel)
    {
       return await _userRepository.UpdateAccessLevel(idUser, newAccessLevel);
    }
}