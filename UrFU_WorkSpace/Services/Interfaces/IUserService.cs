using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IUserService
{
    public Task<AuthenticateResponse> Register(IFormCollection form);
    public Task<AuthenticateResponse> Login(IFormCollection form);
    public Task<string> SendEmailAsync(string email, string subject);
    public Task<bool> UpdateAccessLevel(int idUser, int newAccessLevel);

    public AuthenticateResponse VerifyUser(int idUser, string userCode, string verificationCode);

}