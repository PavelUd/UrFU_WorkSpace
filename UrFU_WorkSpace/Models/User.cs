using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace UrFU_WorkSpace.Models;

public class User
{
    private readonly HttpContext _httpContext;
    private HttpClient _client = new HttpClient();
    private Uri baseAdress = new Uri("https://localhost:7077/api/users");
    private string Pattern = @"^[a-zA-Z0-9._%+-]+@urfu\.me$";

    public User(HttpContext httpContext)
    {
        this._httpContext = httpContext;
    }
    
    private bool IsEmailCorrect(string email) 
        => Regex.IsMatch(email, Pattern);

    public async Task<string> Register(IFormCollection form)
    {
        var dictionary = new Dictionary<string, object>()
        {
            { "firstName", form["first-name"].ToString() },
            { "lastName", form["second-name"].ToString() },
            { "email", form["email"].ToString() },
            { "login", form["login"].ToString() },
            { "password", form["password"].ToString() }
        };
        var json = JsonConvert.SerializeObject(dictionary, Formatting.None);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var responseMessage = await _client.PostAsync("https://localhost:7077/api/users/register", content);
        responseMessage.EnsureSuccessStatusCode();
        var responseBody = await responseMessage.Content.ReadAsStringAsync();
        
        if (responseMessage.StatusCode != HttpStatusCode.OK) 
            return responseBody;
        
        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody)!["result"];
        var jwtToken = JsonConvert.DeserializeObject<Dictionary<string, string>>(result.ToString())!["token"];
        _httpContext.Session.SetString("JwtToken", jwtToken);

        return responseBody;
    }

    public async Task Login(IFormCollection form)
    {
        var dictionary = new Dictionary<string, object>()
        {
            { "email", form["email"].ToString() },
            { "login", form["login"].ToString() },
            { "password", form["password"].ToString() }
        };
        
        var json = JsonConvert.SerializeObject(dictionary, Formatting.None);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var responseMessage = await _client.PostAsync("https://localhost:7077/api/users/login", content);
        responseMessage.EnsureSuccessStatusCode();
        var responseBody = await responseMessage.Content.ReadAsStringAsync();

        if (responseMessage.StatusCode != HttpStatusCode.OK)
            return;
        
        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
        var jwtToken = result["token"];
        _httpContext.Session.SetString("JwtToken", jwtToken.ToString());
        
    }
}