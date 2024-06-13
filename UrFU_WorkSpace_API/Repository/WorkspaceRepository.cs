using System.Linq.Expressions;
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


}