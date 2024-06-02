using System.Drawing;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class ObjectService : IObjectService
{
    private readonly IObjectRepository Repository;
    public ObjectService(IObjectRepository repository)
    {
        Repository = repository;
    }
    
    public bool CreateObjects(int idWorkspace, string jsonObjects)
    {

        var data = JsonHelper.Deserialize<List<Dictionary<string, object>>>(jsonObjects);

        foreach (var obj in data)
        {
            var coordinates = obj["loc"].ToString().Split(" ");
            var size = obj["size"].ToString().Split(" ").Select(int.Parse).ToArray();
            var dict =  CreateObjectDictionary(obj["category"].ToString(), idWorkspace,size, coordinates);
            if (!Repository.CreateObject(dict))
            {
                return false;
            };
        }
        return true;
    }

    public async Task<List<WorkspaceObject>> GetWorkspaceObjects(int idWorkspace)
    {
        return await Repository.GetWorkspaceObjects(idWorkspace);
    }

    private static Dictionary<string, object> CreateObjectDictionary(string category, int idWorkspace, IReadOnlyList<int> size , string[] coordinate)
    {
        return new Dictionary<string, object>()
        {
            { "type", category },
            { "idWorkspace", idWorkspace },
            { "x",  coordinate[0] },
            { "y", coordinate[1] },
            { "height", size[0] },
            { "width", size[1] }
        };
    }
}