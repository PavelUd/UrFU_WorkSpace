using Newtonsoft.Json;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class WorkspaceRepository : IWorkspaceRepository
{
    private Uri BaseAddress;
    
    public WorkspaceRepository(string baseApiAddress)
    {
        BaseAddress = new Uri(baseApiAddress + "/workspaces");
    }

    public async Task<int> CreateWorkspaceAsync(Dictionary<string, object> baseInfo)
    {
        var  responseMessage = HttpRequestSender.SendRequest(BaseAddress + "/add-workspace", RequestMethod.Put, baseInfo).Result;
        responseMessage.EnsureSuccessStatusCode();
        var content = await responseMessage.Content.ReadAsStringAsync();
        
        return int.Parse(content);
    }
    
    public async Task<Workspace> GetWorkspaceAsync(int idWorkspace)
    {
        var responseMessage = HttpRequestSender.SendRequest(BaseAddress + $"/{idWorkspace}", RequestMethod.Get).Result;
        responseMessage.EnsureSuccessStatusCode();
        var content = await responseMessage.Content.ReadAsStringAsync();
        
        return JsonHelper.Deserialize<Workspace>(content);
    }
    
    public async Task<bool> SaveWorkspace()
    {
        var responseMessage = HttpRequestSender.SendRequest(BaseAddress + $"/save", RequestMethod.Post, new Dictionary<string, object>()).Result;
        responseMessage.EnsureSuccessStatusCode();
        var content = await responseMessage.Content.ReadAsStringAsync();
        
        return JsonHelper.Deserialize<bool>(content);
    }
    
    
}