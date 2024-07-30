using System.ComponentModel.DataAnnotations;
using UrFU_WorkSpace_API.Enums;

namespace UrFU_WorkSpace_API.Models;

public class TokenRequest
{
    public TokenRequest(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public TokenRequest()
    {
    }

    public TokenRequest(int code)
    {
        Code = code;
    }

    [Required] public GrantType GrantType { get; set; }

    public string Login { get; set; }
    public string Password { get; set; }

    public int Code { get; set; }
}