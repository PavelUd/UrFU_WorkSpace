using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IUserService
{
    public AuthenticateResponse Authenticate(AuthenticateRequest authenticate);
    public bool IsUserExists(UserCheckRequest user);

    public Task<AuthenticateResponse> Register(User user);
}