namespace UrFU_WorkSpace_API.Models;

public class AuthenticateResponse
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }

    public AuthenticateResponse(User user, string token)
    {
        Id = user.UserId;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Username = user.Login;
        Email = user.Email;
        Token = token;
    }
}