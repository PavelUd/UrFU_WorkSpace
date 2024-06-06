using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Repository;

public class WorkspaceRepository(UrfuWorkSpaceContext context) : BaseRepository<Workspace>(context), IWorkspaceRepository
{
    
    public IEnumerable<Image> GetWorkspaceImages(int workspaceId)
   {
       return FindByCondition(x => x.Id == workspaceId).First().Images;
   }

   public IEnumerable<WorkspaceObject> GetWorkspaceObjects(int workspaceId)
   {
       return FindByCondition(x => x.Id == workspaceId).First().Objects;
   }


   public IEnumerable<WorkspaceAmenity> GetWorkspaceAmenities(int workspaceId)
   {
       return FindByCondition(x => x.Id == workspaceId).First().Amenities;
   }

   public IEnumerable<WorkspaceWeekday> GetWorkspaceOperationMode(int workspaceId)
   {
       return FindByCondition(x => x.Id == workspaceId).First().OperationMode;
   }

   public bool AddWeekday(WorkspaceWeekday weekday)
   {
       _context.OperationMode.Add(weekday);
       return Save();
   }
   public bool AddImage(Image image)
   {
       _context.Images.Add(image);
       return Save();
   }
   
   public bool AddObject(WorkspaceObject obj)
   {
       _context.WorkspaceObjects.Add(obj);
       return Save();
   }
}