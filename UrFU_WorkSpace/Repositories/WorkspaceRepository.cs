using System.Globalization;
using System.Net;
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

    public bool CreateWorkspaceAsync(Workspace baseInfo)
    {
        var  responseMessage = HttpRequestSender.SendRequest(BaseAddress + "/create", RequestMethod.Post, baseInfo).Result;
        return responseMessage.IsSuccessStatusCode;
    }
    
    public async Task<int> UpdateRating(double rating, int idWorkspace)
    {
        var t = rating.ToString(CultureInfo.InvariantCulture);
        var  responseMessage = HttpRequestSender.SendRequest(BaseAddress + $"/{idWorkspace}/update-rating?rating={t}", RequestMethod.Patch).Result;
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
}