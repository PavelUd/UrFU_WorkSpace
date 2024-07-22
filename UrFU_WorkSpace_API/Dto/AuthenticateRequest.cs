using System.ComponentModel.DataAnnotations;

namespace UrFU_WorkSpace_API.Models;

public class AuthenticateRequest
{
    [Required]
    public string Login { get; set; }
    
    [Required]
    public string Password { get; set; }

    public AuthenticateRequest(string login, string password)
    {
        this.Login = login;
        this.Password = password;
    }
}