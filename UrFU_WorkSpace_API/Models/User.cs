using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrFU_WorkSpace_API.Models;

[Table("users")] 
public class User
{
    [Key][Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("first_name")]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("last_name")]
    public string LastName { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("login")]
    public string Login { get; set; }
    
    [Required]
    [MaxLength(20)]
    [PasswordPropertyText]
    [Column("password")]
    public string Password { get; set; }
}