using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Repository.Interfaces;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IWorkspaceRepository : IBaseRepository<Workspace>
{
    public IQueryable<Workspace> IncludeFullInfo(IQueryable<Workspace> query);
    
    public Workspace Replace(Workspace oldEntity);
    public void UpdateRating(int id, double newRating);
}