using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;

namespace UrFU_WorkSpace.Repositories;

public class VerificationCodeRepository : IVerificationCodeRepository
{
    private readonly Uri BaseAddress;

    public VerificationCodeRepository(string baseAddress)
    {
        BaseAddress = new Uri(baseAddress);
    }
    
    public async Task<bool> AddVerificationCode(VerificationCode code)
    {
       var message = await HttpRequestSender.SendRequest(BaseAddress + "/verification-codes/create", RequestMethod.Post, code);
       return message.IsSuccessStatusCode;
    }
    
    public async Task<IEnumerable<VerificationCode>> GetVerificationCodes(int idUser = 0)
    {
        var route = BaseAddress + "/verification-codes";
        if (idUser != 0)
        {
            route += "?idUser=" + idUser;
        }
        var message =await HttpRequestSender.SendRequest(route, RequestMethod.Get);
        var json = await message.Content.ReadAsStringAsync();
        return JsonHelper.Deserialize<IEnumerable<VerificationCode>>(json);
    }
    
    public async Task<VerificationCode> UpdateVerificationCode(VerificationCode code)
    {
        var message = await HttpRequestSender.SendRequest(BaseAddress + "/verification-codes/update", RequestMethod.Put, code);
        var json =await message.Content.ReadAsStringAsync();
        return JsonHelper.Deserialize<VerificationCode>(json);
    }
    
    
}