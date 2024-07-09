using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Primitives;
using MimeKit;
using Newtonsoft.Json;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;

namespace UrFU_WorkSpace.Models;

public class User
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public int AccessLevel { get; set; }
}