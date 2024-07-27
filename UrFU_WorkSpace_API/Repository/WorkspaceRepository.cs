using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Repository;

public class WorkspaceRepository(UrfuWorkSpaceContext context) : BaseRepository<Workspace>(context),
    IWorkspaceRepository
{
    public IQueryable<Workspace> IncludeFullInfo(IQueryable<Workspace> query)
    {
        return query.Include(x => x.OperationMode)
            .Include(x => x.Amenities)
            .Include(x => x.Objects);
    }
}