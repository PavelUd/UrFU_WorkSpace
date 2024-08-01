using System.Globalization;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;

namespace UrFU_WorkSpace.Repositories;

public class WorkspaceRepository(string baseApiAddress) : IWorkspaceRepository
{
    private readonly Uri _baseAddress = new(baseApiAddress + "/workspaces");
    private readonly Dictionary<Type, string> _endpoints = new()
    {
        { typeof(WorkspaceWeekday), "operation-mode" },
        { typeof(WorkspaceObject), "objects" },
        { typeof(WorkspaceAmenity), "amenities" }
    };

    public async Task<Result<int>> CreateWorkspaceAsync(Workspace baseInfo, string token)
    {
        return await HttpRequestSender.HandleJsonRequest<int, Workspace>(_baseAddress + "/create", HttpMethod.Post, baseInfo, token);
    }
    
    public async Task<Result<Workspace>> GetWorkspaceAsync(int idWorkspace, bool isFull)
    {
        var route = _baseAddress + $"/{idWorkspace}?isFull={isFull}";
        return await HttpRequestSender.HandleJsonRequest<Workspace>(route, HttpMethod.Get);
    }
    public async Task<Result<List<Workspace>>> GetAllWorkspacesAsync()
    {
        return await HttpRequestSender.HandleJsonRequest<List<Workspace>>(_baseAddress.ToString(), HttpMethod.Get);
    }

    public async Task<Result<List<TimeSlot>>> GetTimeSlots(int idWorkspace, DateOnly date, TimeType timeType, int? idObjectTemplate)
    {
        var route = _baseAddress + $"/{idWorkspace}/slots?idWorkspace={idWorkspace}&{GetStringDataTime(date, nameof(date))}&type={timeType.ToString().ToLower()}";
        if (idObjectTemplate.HasValue && idObjectTemplate != 0)
        {
            route += $"idObjectTemplate={idObjectTemplate}";
        }
        return await HttpRequestSender.HandleJsonRequest<List<TimeSlot>>(route, HttpMethod.Get);
    }
    public async Task<Result<List<WorkspaceObject>>> FetchWorkspaceObjectsByDateRange(int idWorkspace, int? idTemplate, DateOnly? date, TimeOnly? timeStart, TimeOnly? timeEnd)
    {
        var route = _baseAddress +  $"/{idWorkspace}/objects";
        var queryParams = new List<string>();
        
        if (idTemplate.HasValue && idTemplate != 0) 
            queryParams.Add($"idTemplate={idTemplate}");

        if (date.HasValue)
            queryParams.Add(GetStringDataTime(date.Value, nameof(date)));
                
        foreach (var (value, name) in new[] {(timeStart, nameof(timeStart)), (timeEnd, nameof(timeEnd)) })
        {
            if (value.HasValue) 
                queryParams.Add(GetStringDataTime(value.Value, name));
        }
        if (queryParams.Any())
            route += "?" + string.Join("&", queryParams);

        return await HttpRequestSender.HandleJsonRequest<List<WorkspaceObject>>(route, HttpMethod.Get);


    }

    private static string GetStringDataTime<T>(T data, string name)
    {
        return $"{name}={JsonHelper.Serialize(data).Replace("\"", "")}";
    }
}

