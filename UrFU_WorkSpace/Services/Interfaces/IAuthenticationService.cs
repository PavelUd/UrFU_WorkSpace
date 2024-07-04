using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IAuthenticationService
{
    public Task<AuthenticateResponse> Register(IFormCollection form);
    public Task<AuthenticateResponse> Login(IFormCollection form);
    public Task<string> SendEmailAsync(string email, string subject);

}