using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Helpers;

public static class JwtTokenHelper
{
    public static string GenerateJwtToken(IConfiguration configuration, User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var role = GetRole(user.AccessLevel).ToString();
        var key = Encoding.ASCII.GetBytes(configuration["Secret"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Login", user.Login),
                new Claim("Email", user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("AccessLevel", user.AccessLevel.ToString()),
                new Claim(ClaimTypes.Role, GetRole(user.AccessLevel).ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(90),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static Role GetRole(int accessLevel)
    {
        if (Enum.IsDefined(typeof(Role), accessLevel))
            return (Role)accessLevel;

        return Role.Default;
    }
}