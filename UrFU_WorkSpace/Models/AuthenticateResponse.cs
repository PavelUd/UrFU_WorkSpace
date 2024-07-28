using System.Net;

namespace UrFU_WorkSpace.Models;

public class AuthenticateResponse
{
    public HttpStatusCode StatusCode{ get; init; }
    public string Token { get; set; }

    public string Message { get; set; }
}