using System.Net;

namespace UrFU_WorkSpace_API.Models;

public class AuthenticateResponse
{ 
    public HttpStatusCode StatusCode{ get; init; }
    public string Token { get; set; }

    public string Message { get; set; }

    public AuthenticateResponse(HttpStatusCode statusCode, string message = null, string token = null)
    {
        StatusCode = statusCode;
        Message = message;
        Token = token;
    }
}

