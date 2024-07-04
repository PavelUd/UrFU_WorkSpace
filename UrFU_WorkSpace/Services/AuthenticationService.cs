using System.Net;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class AuthenticationService : IAuthenticationService
{
    private IAuthenticationRepository _authenticationRepository;
    private string Pattern = @"^[a-zA-Z0-9._%+-]+@urfu\.me$";
    
    public AuthenticationService(IAuthenticationRepository authenticationRepository)
    {
        _authenticationRepository = authenticationRepository;
    }

    public async Task<AuthenticateResponse> Register(IFormCollection form)
    {
        var dictionary = new Dictionary<string, object>()
        {
            { "firstName", form["firstName"].ToString() },
            { "lastName", form["secondName"].ToString() },
            { "email", form["email"].ToString() },
            { "login", form["login"].ToString() },
            { "password", form["password"].ToString() }
        };
        return await _authenticationRepository.Register(dictionary);
    }
    
    public async Task<AuthenticateResponse> Login(IFormCollection form)
    {
        var dictionary = new Dictionary<string, object>()
        {
            { "login", form["login"].ToString() },
            { "password", form["password"].ToString() }
        };
        return await _authenticationRepository.Login(dictionary);

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
}