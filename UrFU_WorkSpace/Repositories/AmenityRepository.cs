using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;

namespace UrFU_WorkSpace.Repositories;

public class AmenityRepository : IAmenityRepository
{
    private Uri BaseAddress;
    
    public  AmenityRepository(string baseApiAddress)
    {
        BaseAddress = new Uri(baseApiAddress);
    }
    
    public  bool CreateAmenity(WorkspaceAmenity data)
    {
        var message = HttpRequestSender.SendRequest(BaseAddress + "/workspaces/amenities/create", RequestMethod.Put, data).Result;
        if (!message.IsSuccessStatusCode)
        {
            return false;
        }
        return true;
    }

    public async Task<IEnumerable<AmenityTemplate>> GetAmenityTemplates()
    {
        var responseMessage = HttpRequestSender.SendRequest(BaseAddress + $"/templates/amenities", RequestMethod.Get).Result;
        responseMessage.EnsureSuccessStatusCode();
        
        var data =await responseMessage.Content.ReadAsStringAsync();
        return JsonHelper.Deserialize<List<AmenityTemplate>>(data);
    }
    
    public async Task<List<WorkspaceAmenity>> GetWorkspaceAmenities(int idWorkspace)
    {
        var responseMessage = HttpRequestSender.SendRequest(BaseAddress + $"/workspaces/{idWorkspace}/amenities", RequestMethod.Get).Result;
        responseMessage.EnsureSuccessStatusCode();
        
        var data =await responseMessage.Content.ReadAsStringAsync();
        return JsonHelper.Deserialize<List<WorkspaceAmenity>>(data);
    }
}