using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<AuthenticateResponse> Login(Dictionary<string, object> dictionary);
    
    public Task<AuthenticateResponse> Register(Dictionary<string, object> dictionary);

    public Task<bool> UpdateAccessLevel(int idUser, int newAccessLevel);
}