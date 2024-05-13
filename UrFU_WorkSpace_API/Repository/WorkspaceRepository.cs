using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Repository;

public class WorkspaceRepository(UrfuWorkSpaceContext context) : BaseRepository<Workspace>(context), IWorkspaceRepository
{
   public IEnumerable<WorkspaceImage> GetWorkspaceImages(int workspaceId)
    {
        return _context.WorkspaceImages.Where(image => image.IdWorkspace == workspaceId); 
    }
}