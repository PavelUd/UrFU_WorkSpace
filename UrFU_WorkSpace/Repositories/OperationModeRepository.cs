using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Repositories;

public class OperationModeRepository : IOperationModeRepository
{
    private Uri BaseAddress;
    
    public OperationModeRepository(string baseApiAddress) 
    {
        BaseAddress = new Uri(baseApiAddress + "/workspaces");
    }
    
    public async Task<List<WorkspaceWeekday>> GetWorkspaceOperationModeAsync(int idWorkspace)
    {
        var responseMessage = HttpRequestSender.SendRequest(BaseAddress + $"/{idWorkspace}/operation-mode", RequestMethod.Get).Result;
        responseMessage.EnsureSuccessStatusCode();
        var content = await responseMessage.Content.ReadAsStringAsync();
        
        return JsonHelper.Deserialize<List<WorkspaceWeekday>>(content);
    }
    
    public bool CreateWeekday(WorkspaceWeekday weekday)
    {
        var message = HttpRequestSender.SendRequest(BaseAddress + "/add-weekday", RequestMethod.Put, weekday).Result;
        if (!message.IsSuccessStatusCode)
        {
                return false;
        }
        return true;
    }
}