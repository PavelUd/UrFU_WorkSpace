using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UrFU_WorkSpace.Helpers;

public static class JwtTokenDecoder
{
    
    private static Dictionary<string, string> Decode(string token)
    {
        return new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.ToDictionary(t => t.Type,t => t.Value);
    }

    public static string GetUserName(string token)
    {
        if (token == null)
        {
            return "";
        }
        var info = Decode(token);
        return info["Login"];
    }
    
    public static string GetUserId(string token)
    {
        if (token == null)
        {
            return "";
        }
        var info = Decode(token);
        return info["Id"];
    }
    
}