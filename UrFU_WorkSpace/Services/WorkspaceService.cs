
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;
using UrFU_WorkSpace.Services.Interfaces;
using Reservation = UrFU_WorkSpace.Models.Reservation;
using Workspace = UrFU_WorkSpace.Models.Workspace;
using WorkspaceAmenity = UrFU_WorkSpace.Models.WorkspaceAmenity;
using WorkspaceObject = UrFU_WorkSpace.Models.WorkspaceObject;
using WorkspaceWeekday = UrFU_WorkSpace.Models.WorkspaceWeekday;

namespace UrFU_WorkSpace.Services;

public class WorkspaceService : IWorkspaceService
{
    private IWorkspaceRepository Repository;
    private IObjectService ObjectService;
    private IOperationModeService OperationModeService;
    private IReservationService ReservationService;
    private IAmenityService AmenityService;
   
    public WorkspaceService(IWorkspaceRepository repository, 
        IObjectService objectService, 
        IOperationModeService operationModeService,
        IReservationService reservationService, IAmenityService amenityService)
    {

        AmenityService = amenityService;
        Repository = repository;
        ReservationService = reservationService;
        ObjectService = objectService;
        OperationModeService = operationModeService;
    }

   public async Task<Result<Workspace>> GetWorkspace(int idWorkspace)
   {
       return await Repository.GetWorkspaceAsync(idWorkspace, true);
   }
   
   public Workspace ConstructWorkspace(Dictionary<string, object> baseInfo, IEnumerable<WorkspaceAmenity> amenities, IEnumerable<WorkspaceObject> objects, IEnumerable<WorkspaceWeekday> operationMode, int idUser)
   {
       var workspace = new Workspace()
       {
           IdCreator = idUser,
           Privacy = 0,
           Rating = 0,
           Name = baseInfo["name"].ToString(),
           Description = baseInfo["description"].ToString(),
           Institute = baseInfo["institute"].ToString(),
           Address = baseInfo["address"].ToString(),
           Objects = objects,
           Amenities = amenities,
           OperationMode = operationMode,
       };
       return workspace;
   }
   
   public  Task<Result<int>> CreateWorkspace(int idUser,Dictionary<string, object> baseInfo,List<(string, string)> operationModeJson, List<int> idTemplates, string jsonObjects, IFormFileCollection uploads,
       IWebHostEnvironment appEnvironment, string token)
   {
       var operationMode =  OperationModeService.ConstructOperationMode(operationModeJson, idUser);
       var objects = ObjectService.ConstructWorkspaceObjects(jsonObjects);
       var amenities = AmenityService.ConstructWorkspaceAmenities(idTemplates);
       var workspace = ConstructWorkspace(baseInfo,amenities.AsEnumerable(), objects,operationMode, idUser);
       return  Repository.CreateWorkspaceAsync(workspace, token);
   }
   
   public async Task<Result<List<TimeSlot>>> GetWorkspaceTimeSlots(int idWorkspace, DateTime date, TimeType timeType, int idTemplate)
   {
       return await Repository.GetTimeSlots(idWorkspace, new DateOnly(date.Year, date.Month, date.Day), timeType, idTemplate);
   }
    
   public async Task<Result<List<WorkspaceObject>>> GetWorkspaceObjects(int idWorkspace, int? idTemplate, DateOnly? date, TimeOnly? timeStart, TimeOnly? timeEnd)
   {
       return await ObjectService.GetWorkspaceObjects(idWorkspace,idTemplate, date, timeStart, timeEnd);
   }
   
   public async Task<Result<List<Workspace>>> GetAllWorkspaces()
   {
       return await Repository.GetAllWorkspacesAsync();
   }
}