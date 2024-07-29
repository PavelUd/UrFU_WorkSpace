using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Repository.Interfaces;

public interface IWorkspaceProvider : IBaseProvider<Workspace>
{
    public IQueryable<Workspace> IncludeFullInfo(IQueryable<Workspace> query);
}