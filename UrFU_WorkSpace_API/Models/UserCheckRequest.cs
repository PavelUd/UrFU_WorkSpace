using System.ComponentModel.DataAnnotations;

namespace UrFU_WorkSpace_API.Models;

public class UserCheckRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Login { get; set; }
}