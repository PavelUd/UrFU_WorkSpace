using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class ObjectRepository : IObjectRepository
{
    private Uri BaseAddress;
    
    public ObjectRepository(string baseApiAddress)
    {
        BaseAddress = new Uri(baseApiAddress + "/workspaces");
    }
    
    public  bool CreateObject(Dictionary<string, object> data)
    {
        var message = HttpRequestSender.SendRequest(BaseAddress + "/add-object", RequestMethod.Put, data).Result;
           if (!message.IsSuccessStatusCode)
           {
               return false;
           }
        return true;
    }
    
    public async Task<List<WorkspaceObject>> GetWorkspaceObjects(int idWorkspace)
    {
        var responseMessage = HttpRequestSender.SendRequest(BaseAddress + $"/{idWorkspace}/objects", RequestMethod.Get).Result;
        responseMessage.EnsureSuccessStatusCode();
        
        var data =await responseMessage.Content.ReadAsStringAsync();
        return JsonHelper.Deserialize<List<WorkspaceObject>>(data);
    }
}