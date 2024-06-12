using System.Drawing;
using System.Globalization;
using System.Linq.Expressions;
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

    public List<WorkspaceObject> ConstructWorkspaceObjects(string jsonObjects, int id = 0)
    {
        var data = JsonHelper.Deserialize<List<Dictionary<string, object>>>(jsonObjects);
        var objects = new List<WorkspaceObject>();
        foreach (var obj in data)
        {
            var coordinates = obj["loc"].ToString().Split(" ");
            var size = obj["size"].ToString().Split(" ").Select(int.Parse).ToArray();
            objects.Add(CreateObject(size, coordinates,  int.Parse(obj["idTemplate"].ToString())));
        }

        return objects;
    }
    
    public async Task<IEnumerable<ObjectTemplate>> GetObjectTemplates()
    {
        return await Repository.GetObjectTemplates();
    }
    
    public bool CreateObjects(List<WorkspaceObject> objects)
    {
        foreach (var obj in objects)
        {
            if (!Repository.CreateObject(obj))
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
    
    public async Task<List<WorkspaceObject>> GetWorkspaceObjectsByCondition(int idWorkspace, Expression<Func<WorkspaceObject, bool>> expression)
    {
        var workspaceObjects =await  GetWorkspaceObjects(idWorkspace);
        return workspaceObjects.Where(expression.Compile()).ToList();
    }

    private static WorkspaceObject CreateObject(IReadOnlyList<int> size , string[] coordinate, int idTemplate, int id = 0)
    {
        return new WorkspaceObject()
        {
            Id = 0,
            IdTemplate = idTemplate,
            X = double.Parse(coordinate[0],NumberStyles.Any, CultureInfo.InvariantCulture),
            Y = double.Parse(coordinate[1],NumberStyles.Any, CultureInfo.InvariantCulture),
            Height =  size[0],
            Width = size[1]
        };
    }
}