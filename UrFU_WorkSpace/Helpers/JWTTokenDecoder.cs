using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Helpers;

public static class JwtTokenDecoder
{
    
    public static User? Decode(string? token)
    {
        if (token == null)
        {
            return null;
        }
        var userStr= new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.ToDictionary(t => t.Type,t => t.Value);
        
        return new User()
        {
            Id = int.Parse(userStr["Id"]),
            Login = userStr["Login"],
            AccessLevel = int.Parse(userStr["AccessLevel"]),
            Email = userStr["Email"],
            FirstName = userStr["FirstName"],
            LastName = userStr["LastName"]
        };

    }

    public static string GetUserName(string token)
    {
        if (token == null)
        {
            return "";
        }
        var info = Decode(token);
        return info.Login;
    }

    public static int GetUserAccessLevel(string token)
    {
        if (token == null)
        {
            return 0;
        }
        var info = Decode(token);
        return info.AccessLevel;
    }
    
    public static int GetUserId(string token)
    {
        if (token == null)
        {
            return 0;
        }
        var info = Decode(token);
        return info.Id;
    }
    
}