using System.Drawing;
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

    public List<WorkspaceObject> ConstructWorkspaceObjects(string jsonObjects, int idWorkspace = 0, int id = 0)
    {
        var data = JsonHelper.Deserialize<List<Dictionary<string, object>>>(jsonObjects);
        var objects = new List<WorkspaceObject>();
        foreach (var obj in data)
        {
            var coordinates = obj["loc"].ToString().Split(" ");
            var size = obj["size"].ToString().Split(" ").Select(int.Parse).ToArray();
            objects.Add(CreateObject(obj["category"].ToString(), idWorkspace,size, coordinates, id));
        }

        return objects;
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

    private static WorkspaceObject CreateObject(string category, int idWorkspace, IReadOnlyList<int> size , string[] coordinate, int id)
    {
        return new WorkspaceObject()
        {
            Id = id,
            IdTemplate = id,
            IdWorkspace = idWorkspace,
            X = double.Parse(coordinate[0]),
            Y = double.Parse(coordinate[1]),
            Height =  size[0],
            Width = size[1]
        };
    }
}