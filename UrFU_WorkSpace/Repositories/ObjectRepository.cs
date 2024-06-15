using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Repositories;

public class ObjectRepository : IObjectRepository
{
    private Uri BaseAddress;
    
    public ObjectRepository(string baseApiAddress)
    {
        BaseAddress = new Uri(baseApiAddress);
    }
    
    public  bool CreateObject(WorkspaceObject data)
    {
        var message = HttpRequestSender.SendRequest(BaseAddress + "/workspaces/create", RequestMethod.Put, data).Result;
           if (!message.IsSuccessStatusCode)
           {
               return false;
           }
        return true;
    }

    public async Task<IEnumerable<ObjectTemplate>> GetObjectTemplates()
    {
        var responseMessage = HttpRequestSender.SendRequest(BaseAddress + $"/templates/objects", RequestMethod.Get).Result;
        responseMessage.EnsureSuccessStatusCode();
        
        var data =await responseMessage.Content.ReadAsStringAsync();
        return JsonHelper.Deserialize<List<ObjectTemplate>>(data);
    }
    
    public async Task<List<WorkspaceObject>> GetWorkspaceObjects(int idWorkspace)
    {
        var responseMessage = HttpRequestSender.SendRequest(BaseAddress + $"/workspaces/{idWorkspace}/objects", RequestMethod.Get).Result;
        responseMessage.EnsureSuccessStatusCode();
        
        var data =await responseMessage.Content.ReadAsStringAsync();
        return JsonHelper.Deserialize<List<WorkspaceObject>>(data);
    }
}