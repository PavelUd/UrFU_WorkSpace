using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IWorkspaceRepository : IBaseRepository<Workspace>
{
    public IQueryable<Workspace> IncludeFullInfo(IQueryable<Workspace> query);
}