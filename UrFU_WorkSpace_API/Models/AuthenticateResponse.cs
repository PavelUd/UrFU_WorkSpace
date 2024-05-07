namespace UrFU_WorkSpace_API.Models;

public class AuthenticateResponse
{ 
    public string Token { get; set; }

    public AuthenticateResponse(string token)
    {
        Token = token;
    }
}