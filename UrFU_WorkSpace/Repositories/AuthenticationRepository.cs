using System.Net;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;

namespace UrFU_WorkSpace.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private Uri BaseAddress;

    public AuthenticationRepository(string baseApiAddress)
    {
        BaseAddress = new Uri(baseApiAddress + "/users");
    }
    public async Task<AuthenticateResponse> Login(Dictionary<string, object> dictionary)
    {
        var responseMessage = await HttpRequestSender.SendRequest(BaseAddress + "/login", RequestMethod.Post, dictionary);
        var data = await responseMessage.Content.ReadAsStringAsync();
        return JsonHelper.Deserialize<AuthenticateResponse>(data);
    }

    public async Task<AuthenticateResponse> Register(Dictionary<string, object> dictionary)
    {
        var responseMessage = await HttpRequestSender.SendRequest(BaseAddress + "/register", RequestMethod.Post, dictionary);
        var data = await responseMessage.Content.ReadAsStringAsync();
        return JsonHelper.Deserialize<AuthenticateResponse>(data);
    }
}