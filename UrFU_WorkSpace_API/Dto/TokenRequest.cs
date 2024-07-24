using System.ComponentModel.DataAnnotations;

namespace UrFU_WorkSpace_API.Models;

public class TokenRequest
{
    [Required]
    public string GrantType { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    public int Code { get; set; }

    public TokenRequest(string login, string password)
    {
        this.Login = login;
        this.Password = password;
    }

    public TokenRequest()
    {
        
    }
    
    public TokenRequest(int code)
    {
        this.Code = code;
    }
}