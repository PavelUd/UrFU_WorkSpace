using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Repository;

public class WorkspaceAmenityRepository : BaseRepository<WorkspaceAmenity>
{
    public WorkspaceAmenityRepository(UrfuWorkSpaceContext context) : base(context)
    {
    }
}