using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Repository.Interfaces;

namespace UrFU_WorkSpace_API.Repository;

public class WorkspaceRepository(UrfuWorkSpaceContext context) : BaseRepository<Workspace>(context),
    IWorkspaceRepository, IWorkspaceProvider
{
    public IQueryable<Workspace> IncludeFullInfo(IQueryable<Workspace> query)
    {
        return query.Include(x => x.OperationMode).AsNoTracking()
            .Include(x => x.Amenities).AsNoTracking()
            .Include(x => x.Objects).AsNoTracking();
    }
    public Workspace Replace(Workspace entity)
    {
        RemoveDiffWorkspaceComponents(entity.Id, entity.Objects);
        RemoveDiffWorkspaceComponents(entity.Id, entity.Amenities);
        RemoveDiffWorkspaceComponents(entity.Id, entity.OperationMode);
        RemoveImages(entity.Id);
        _context.Set<Workspace>().Update(entity);
        _context.SaveChanges();
        return entity;
    }

    private void RemoveDiffWorkspaceComponents<T>(int idWorkspace, IEnumerable<T> workspaceComponents) where T : class, IWorkspaceComponent
    {
        var oldComponents = _context.Set<T>().Where(x => x.IdWorkspace == idWorkspace).IgnoreAutoIncludes().AsNoTracking().AsEnumerable();
        var diff = oldComponents.ExceptBy(workspaceComponents.Select(o => o.Id), o => o.Id);
        _context.Set<T>().RemoveRange(diff);
    }
    
    private void RemoveImages(int idWorkspace) 
    {
        var images = _context.Set<WorkspaceImage>().Where(x => x.IdOwner== idWorkspace).AsNoTracking();
        _context.Set<WorkspaceImage>().RemoveRange(images);
    }
}