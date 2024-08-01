using System.Linq.Expressions;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IObjectService
{
    public Task<Result<List<WorkspaceObject>>> GetWorkspaceObjects(int idWorkspace, int? idTemplate,
        DateOnly? date, TimeOnly? timeStart, TimeOnly? timeEnd);

    public List<WorkspaceObject> ConstructWorkspaceObjects(string jsonObjects, int id = 0);
}