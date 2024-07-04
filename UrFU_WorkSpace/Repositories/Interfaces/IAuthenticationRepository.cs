using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Repositories.Interfaces;

public interface IAuthenticationRepository
{
    public Task<AuthenticateResponse> Login(Dictionary<string, object> dictionary);
    
    public Task<AuthenticateResponse> Register(Dictionary<string, object> dictionary);
}