using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services;

public class AuthenticationService
{
    private Uri BaseAddress;
    private string Pattern = @"^[a-zA-Z0-9._%+-]+@urfu\.me$";

    public AuthenticationService(string baseApiAddress)
    {
        BaseAddress = new Uri(baseApiAddress + "/oauth/");
    }
    
    public async Task<Result<None>> Register(IFormCollection form)
    {
        var dictionary = new Dictionary<string, object>()
        {
            { "firstName", form["first-name"].ToString() },
            { "lastName", form["second-name"].ToString() },
            { "email", form["email"].ToString() },
            { "login", form["login"].ToString() },
            { "password", form["password"].ToString() }
        };
        return await HttpRequestSender.HandleJsonRequest<None, Dictionary<string, object>>(BaseAddress + "register", HttpMethod.Post, dictionary, null);
    }
    
    public async Task<Result<JwtToken>> GetToken(IFormCollection form)
    {
        var dictionary = new Dictionary<string, object>()
        {
            {"grantType", form["grantType"].ToString()},
            { "login", form["login"].ToString() },
            { "password", form["password"].ToString() },
        };

        var code = form["code"].ToString() != "" ? int.Parse(form["code"].ToString()) : 0;
        dictionary.Add("code", code);
        return await HttpRequestSender.HandleJsonRequest<JwtToken, Dictionary<string, object>>(BaseAddress + "token", HttpMethod.Post, dictionary, null);

    }
    
    
    
    
}