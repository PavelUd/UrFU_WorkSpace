using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Primitives;
using MimeKit;
using Newtonsoft.Json;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;

namespace UrFU_WorkSpace.Models;

public class User(HttpContext httpContext)
{
    private Uri baseAdress = new Uri("https://localhost:7077/api/users");
    private string Pattern = @"^[a-zA-Z0-9._%+-]+@urfu\.me$";

    private bool IsEmailCorrect(string email) 
        => Regex.IsMatch(email, Pattern);

    public async Task Register(IFormCollection form)
    {
        
        var dictionary = new Dictionary<string, object>()
        {
            { "firstName", form["firstName"].ToString() },
            { "lastName", form["secondName"].ToString() },
            { "email", form["email"].ToString() },
            { "login", form["login"].ToString() },
            { "password", form["password"].ToString() }
        };

        var responseMessage = await HttpRequestSender.SendRequest(baseAdress + "/register", RequestMethod.Post, dictionary);
        WriteJwtToken(responseMessage);

    }

    public async Task<bool> CheckUserExistence(IFormCollection form)
    {
        var dictionary = new Dictionary<string, object>()
        {
            { "email", form["email"].ToString() },
            { "login", form["login"].ToString() },
        };

        var responseMessage = await HttpRequestSender.SendRequest(baseAdress + "/check-user-existence", RequestMethod.Post, dictionary);
        var responseBody = await responseMessage.Content.ReadAsStringAsync();
        return bool.Parse(responseBody);
    }
    
    public async Task<string> Login(IFormCollection form)
    {
        var random = new Random();
        var dictionary = new Dictionary<string, object>()
        {
            { "login", form["login"].ToString() },
            { "password", form["password"].ToString() }
        };
        
        var responseMessage = await HttpRequestSender.SendRequest(baseAdress + "/login", RequestMethod.Post, dictionary);
        if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
        {
            return "Неправильный логин или пароль";
        }
        WriteJwtToken(responseMessage);
        return "";
    }

    private async void WriteJwtToken(HttpResponseMessage responseMessage)
    {
        var responseBody = await responseMessage.Content.ReadAsStringAsync();
        if (responseMessage.StatusCode != HttpStatusCode.OK)
            return;
        
        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
        if (!result.ContainsKey("token"))
        {
            result = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["result"].ToString());
        }
        var jwtToken = result["token"].ToString();
        httpContext.Session.SetString("JwtToken", jwtToken);
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