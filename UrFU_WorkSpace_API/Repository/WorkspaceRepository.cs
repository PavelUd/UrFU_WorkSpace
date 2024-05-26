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

   public IEnumerable<WorkspaceObject> GetWorkspaceObjects(int workspaceId)
   {
       return _context.WorkspaceObjects.Where(obj => obj.IdWorkspace == workspaceId);
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

   public IEnumerable<WorkspaceWeekday> GetWorkspaceOperationMode(int workspaceId)
   {
       return _context.OperationMode.Where(om => om.IdWorkspace == workspaceId);
   }

   public bool AddWeekday(WorkspaceWeekday weekday)
   {
       _context.OperationMode.Add(weekday);
       return Save();
   }
   
   public bool AddObject(WorkspaceObject obj)
   {
       _context.WorkspaceObjects.Add(obj);
       return Save();
   }
   
   public bool AddImage(WorkspaceImage image)
   {
       _context.WorkspaceImages.Add(image);
       return Save();
   }
}