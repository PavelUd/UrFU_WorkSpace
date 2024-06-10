using System.Linq.Expressions;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IObjectService
{
    public bool CreateObjects(List<WorkspaceObject> objects);

    public Task<List<WorkspaceObject>> GetWorkspaceObjectsByCondition(int idWorkspace,
        Expression<Func<WorkspaceObject, bool>> expression);
    public Task<List<WorkspaceObject>> GetWorkspaceObjects(int idWorkspace);
    public List<WorkspaceObject> ConstructWorkspaceObjects(string jsonObjects, int idWorkspace = 0, int id = 0);


}