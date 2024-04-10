using System.ComponentModel.DataAnnotations;

namespace UrFU_WorkSpace_API.Models;

public class AuthenticateRequest
{

    public string Login { get; set; }
    
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}