using System.Drawing;
using System.Xml.Linq;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;
using Image = UrFU_WorkSpace.Models.Image;
using Reservation = UrFU_WorkSpace.Models.Reservation;
using Size = Npgsql.Internal.Size;
using Workspace = UrFU_WorkSpace.Models.Workspace;
using WorkspaceObject = UrFU_WorkSpace.Models.WorkspaceObject;
using WorkspaceWeekday = UrFU_WorkSpace.Models.WorkspaceWeekday;

namespace UrFU_WorkSpace.Services;

public class WorkspaceService : IWorkspaceService
{
    private IWorkspaceRepository Repository;
    private IObjectService ObjectService;
    private IOperationModeService OperationModeService;
    private IImageService ImageService;
    private IReservationService ReservationService;
   
    public WorkspaceService(IWorkspaceRepository repository, 
        IObjectService objectService, 
        IOperationModeService operationModeService,
        IImageService imageService, IReservationService reservationService)
    {
        Repository = repository;
        ReservationService = reservationService;
        ObjectService = objectService;
        OperationModeService = operationModeService;
        ImageService = imageService;
    }

   public Workspace GetWorkspace(int idWorkspace)
   {
       return Repository.GetWorkspaceAsync(idWorkspace).Result;
   }
   
   public Workspace ConstructWorkspace(Dictionary<string, object> baseInfo, IEnumerable<WorkspaceObject> objects, IEnumerable<WorkspaceWeekday> operationMode, IEnumerable<Image> images, int idUser)
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
           Amenities = new List<WorkspaceAmenity>(),
           OperationMode = operationMode,
           Images = images,
       };
       return workspace;
   }

   public bool CreateWorkspace(int idUser,Dictionary<string, object> baseInfo,List<(string, string)> operationModeJson, string jsonObjects, IFormFileCollection uploads,
       IWebHostEnvironment appEnvironment)
   {
       var urls = ImageService.SaveImages(appEnvironment, uploads);
       var images = ImageService.ConstructImages(urls);
       var operationMode =  OperationModeService.ConstructOperationMode(operationModeJson, idUser);
       var objects = ObjectService.ConstructWorkspaceObjects(jsonObjects);

       var workspace = ConstructWorkspace(baseInfo, objects,operationMode, images, idUser);
       return  Repository.CreateWorkspaceAsync(workspace).Result;
   }
   
   public List<TimeSlot> GetWorkspaceTimeSlots(int idWorkspace, DateTime date, TimeType timeType, int idTemplate)
   {
       var operationMode = WeekdayForDate(idWorkspace, date);
       var objects = ObjectService.GetWorkspaceObjectsByCondition(idWorkspace, x => x.Template.Id == idTemplate || idTemplate == 0).Result;
       var reservations = ReservationService.GetReservations(idWorkspace, new DateOnly(date.Year, date.Month, date.Day)).Result;
       return GenerateTimeSlots(operationMode,reservations, objects, timeType);
   }

   public List<WorkspaceObject> GetReservedObjects(TimeOnly start, TimeOnly end, int idWorkspace, DateTime date,int idTemplate)
   {
       var reservations = ReservationService.GetReservations(idWorkspace, new DateOnly(date.Year, date.Month, date.Day)).Result;
       var objects = ObjectService.GetWorkspaceObjectsByCondition(idWorkspace, x => x.Template.Id == idTemplate || idTemplate == 0).Result;
       return ReservationService.UpdateReservationStatus(start, end, reservations, objects);
   }
   
   private WorkspaceWeekday WeekdayForDate(int idWorkspace, DateTime date)
   {
       var dayOfWeek = date.DayOfWeek;
       var operationMode = OperationModeService.GetOperationMode(idWorkspace);
       var weekday = operationMode.FirstOrDefault(w => (int)dayOfWeek == w.WeekDayNumber % 7);
       return weekday ?? new WorkspaceWeekday();
   }

   private List<TimeSlot> GenerateTimeSlots(WorkspaceWeekday operationMode,List<Reservation> reservations, List<WorkspaceObject> objects, TimeType timeType)
   {
       var timeSlots = new List<TimeSlot>();
       var timeStart = operationMode.TimeStart;
       var slotsLen = (operationMode.TimeEnd - operationMode.TimeStart).TotalMinutes / (int)timeType;

       for (var i = 0; i < slotsLen; i++)
       {
           var timeEnd = timeStart.AddMinutes((int)timeType);
           
           if (timeEnd > operationMode.TimeEnd) continue;
           
           var allReserved = ReservationService.UpdateReservationStatus(timeStart, timeEnd, reservations, objects).All(x => x.IsReserve);
           timeSlots.Add(new TimeSlot(timeStart, timeEnd, allReserved));
           timeStart = timeEnd;
       }

       return timeSlots;
   }
}