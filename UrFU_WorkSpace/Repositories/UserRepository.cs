using System.Net;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;
using IUserRepository = UrFU_WorkSpace.Repositories.Interfaces.IUserRepository;

namespace UrFU_WorkSpace.Repositories;

public class UserRepository : IUserRepository
{
    private Uri BaseAddress;

    public UserRepository(string baseApiAddress)
    {
        BaseAddress = new Uri(baseApiAddress + "/users/");
    }
    public async Task<AuthenticateResponse> Login(Dictionary<string, object> dictionary)
    {
        var responseMessage = await HttpRequestSender.SendRequest(BaseAddress + "login", RequestMethod.Post, dictionary);
        var data = await responseMessage.Content.ReadAsStringAsync();
        return JsonHelper.Deserialize<AuthenticateResponse>(data);
    }

    public async Task<AuthenticateResponse> Register(Dictionary<string, object> dictionary)
    {
        var responseMessage = await HttpRequestSender.SendRequest(BaseAddress + "register", RequestMethod.Post, dictionary);
        var data = await responseMessage.Content.ReadAsStringAsync();
        return JsonHelper.Deserialize<AuthenticateResponse>(data);
    }
    
    public async Task<bool> UpdateAccessLevel(int idUser,int newAccessLevel)
    {
        var route = BaseAddress + idUser.ToString() + "/update-access-level?accessLevel=" + newAccessLevel;
        var responseMessage = await HttpRequestSender.SendRequest(route,
            RequestMethod.Patch, newAccessLevel);
        return responseMessage.IsSuccessStatusCode;
    }
}