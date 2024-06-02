using System.Drawing;
using System.Xml.Linq;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;
using Size = Npgsql.Internal.Size;

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
   
   public int AddBaseWorkspaceInfo(IFormCollection form, int idUser)
   {
       var baseInfo = new Dictionary<string, object>()
       {
           {"name", form["name"].ToString() },
           {"description", form["description"].ToString()},
           {"rating", 0},
           {"institute", form["institute"].ToString()},
           {"address", form["address"].ToString()},
           {"privacy", 0},
           {"idCreator", idUser}
       };
       return Repository.CreateWorkspaceAsync(baseInfo).Result;
   }

   public bool CreateWorkspace(int idUser,IFormCollection form, IFormFileCollection uploads, IWebHostEnvironment appEnvironment)
   {
       var urls = ImageService.SaveImages(appEnvironment, uploads);
        
       var idCreatedWorkspace = AddBaseWorkspaceInfo(form, idUser);
       if (idCreatedWorkspace == 0)
       {
           return false;
       }
       var imagesIsSaved = ImageService.CreateImages(idCreatedWorkspace ,urls);
       var objIsSaved = ObjectService.CreateObjects(idCreatedWorkspace ,form["objects"]);
       var weekDaysIsSaved = OperationModeService.CreateOperationMode(form ,idCreatedWorkspace);
       
       return objIsSaved && weekDaysIsSaved && imagesIsSaved;
   }
   
   public List<TimeSlot> GetWorkspaceTimeSlots(int idWorkspace, DateTime date, TimeType timeType, string typeObject)
   {
       var operationMode = WeekdayForDate(idWorkspace, date);
       var objects = ObjectService.GetWorkspaceObjects(idWorkspace).Result;
       var reservations = ReservationService.GetReservations(idWorkspace, new DateOnly(date.Year, date.Month, date.Day)).Result;
       return GenerateTimeSlots(operationMode,reservations, objects, timeType);
   }

   public List<WorkspaceObject> GetReservedObjects(TimeOnly start, TimeOnly end, int idWorkspace, DateTime date)
   {
       var reservations = ReservationService.GetReservations(idWorkspace, new DateOnly(date.Year, date.Month, date.Day)).Result;
       var objects = ObjectService.GetWorkspaceObjects(idWorkspace).Result;
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