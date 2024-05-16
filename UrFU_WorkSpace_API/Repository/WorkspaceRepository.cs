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
   
   public IEnumerable<WorkspaceAmenity> GetWorkspaceAmenities(int workspaceId)
   {
       return _context.WorkspaceAmenities
           .Join(_context.AmenityDetails,wa => wa.IdAmenity, ad => ad.Id, (wa, ad) => new { Amenity = wa, Detail = ad })
           .Select(x => new WorkspaceAmenity
           {
               Id = x.Amenity.Id,
               IdAmenity = x.Amenity.IdAmenity,
               IdWorkspace = x.Amenity.IdWorkspace,
               Detail = x.Detail
           }).Where(x => x.IdWorkspace == workspaceId); 
   }
}