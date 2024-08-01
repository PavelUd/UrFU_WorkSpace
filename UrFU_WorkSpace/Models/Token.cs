using System.Net;

namespace UrFU_WorkSpace.Models;

public class JwtToken
{
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
}