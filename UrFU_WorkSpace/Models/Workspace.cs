using System.Net;
using Newtonsoft.Json;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;

namespace UrFU_WorkSpace.Models;

public class Workspace
{
    private static Uri baseAdress = new Uri("https://localhost:7077/api/workspaces");
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public double Rating { get; set; }
    
    public string Address { get; set; }
    
    public string Institute { get; set; }
    
    public int Privacy { get; set; }
    
    public int IdCreator { get; set; }
    
    public IEnumerable<WorkspaceImage> Images { get; set; }
    public IEnumerable<UrFU_WorkSpace_API.Models.WorkspaceObject> Objects { get; set; }
    public IEnumerable<WorkspaceWeekday> OperationMode { get; set; }
    public IEnumerable<WorkspaceAmenity> Amenities { get; set; }

    public static List<TimeSlot> GetWorkspaceTimeSlots(int idWorkspace, DateTime date, TimeType timeType, string typeObject)
    {
        var dayOfWeek = date.DayOfWeek;
        var op = GetWorkSpaceOperationMode(idWorkspace);
        var weekday = op.FirstOrDefault(w => (int)dayOfWeek == w.WeekDayNumber % 7);
        var time = weekday.TimeEnd - weekday.TimeStart;
        var slotsLen = time.TotalMinutes / (int)timeType;
        var timeSlots = new List<TimeSlot>();
        var timeStart = weekday.TimeStart;
        var reservations = GetWorkSpaceReservations(idWorkspace, new DateOnly(date.Year, date.Month, date.Day));
        var objects = GetWorkSpaceObjects(idWorkspace);
        for (var i = 0; i < slotsLen; i++)
        {
            var timeEnd = timeStart.AddMinutes((int)timeType);
            if (timeEnd <= weekday.TimeEnd)
            {
                var t = CheckReservations(timeStart, timeEnd, reservations, objects).All(x => x.IsReserve);
                timeSlots.Add(new TimeSlot(timeStart, timeEnd, t));
                timeStart = timeEnd;
            }
        }

        return timeSlots;
    }

    public static List<WorkspaceObject> CheckReservations(TimeOnly start, TimeOnly end, List<Reservation> reservations, List<WorkspaceObject> objects)
    {
        var objs= new List<WorkspaceObject>();
        foreach (var obj in objects)
        {
            var flag = reservations.Any(x => x.IdObject == obj.IdObject && !(x.TimeStart >= end || x.TimeEnd <= start));
            obj.IsReserve = flag;
           objs.Add(obj);
        }

        return objs;
    }

    public static async Task<Workspace> GetWorkSpace(int idWorkspace)
    {
        var workspace = new Workspace();
        var responseMessage = HttpRequestSender.SentGetRequest(baseAdress + $"/{idWorkspace}").Result;
        var settings = new JsonSerializerSettings(){};
        settings.Converters.Add(new TimeOnlyJsonConverter());
        if (responseMessage.IsSuccessStatusCode)
        {
            var data = responseMessage.Content.ReadAsStringAsync().Result;
            workspace = JsonConvert.DeserializeObject<Workspace>(data, settings);
        }

        return workspace;
    }
    public static List<WorkspaceObject> GetWorkSpaceObjects(int idWorkspace)
    {
        var reservations = new List<WorkspaceObject>();
        var operationModeMessage = HttpRequestSender.SentGetRequest(baseAdress + $"/{idWorkspace}/objects").Result;
        if (!operationModeMessage.IsSuccessStatusCode)
            return reservations;
        
        var data = operationModeMessage.Content.ReadAsStringAsync().Result;
        reservations = JsonConvert.DeserializeObject<List<WorkspaceObject>>(data);

        return reservations;
    }
    public static List<Reservation> GetWorkSpaceReservations(int idWorkspace, DateOnly date)
    {
        var reservations = new List<Reservation>();
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new TimeOnlyJsonConverter());
        settings.Converters.Add(new DateOnlyJsonConverter());
        var jsonDate =  JsonConvert.SerializeObject(date).Replace("\"", "");
        var route = "https://localhost:7077/api/reservations?idWorkspace=" + idWorkspace + "&date=" + jsonDate;
        var operationModeMessage = HttpRequestSender.SentGetRequest(route).Result;
        if (operationModeMessage.IsSuccessStatusCode)
        {
            var data = operationModeMessage.Content.ReadAsStringAsync().Result;
            reservations = JsonConvert.DeserializeObject<List<Reservation>>(data, settings);

        }

        return reservations;
    }
    public static List<WorkspaceWeekday> GetWorkSpaceOperationMode(int idWorkspace)
    {
        var operationMode = new List<WorkspaceWeekday>();
        var operationModeMessage = HttpRequestSender.SentGetRequest(baseAdress + $"/{idWorkspace}/operation-mode").Result;
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new TimeOnlyJsonConverter());
        if (operationModeMessage.IsSuccessStatusCode)
        {
            var data = operationModeMessage.Content.ReadAsStringAsync().Result;
            operationMode = JsonConvert.DeserializeObject<List<WorkspaceWeekday>>(data, settings);
        }

        return operationMode;
    }

    public static List<WorkspaceObject> GetReservationObjects(TimeOnly start, TimeOnly end, int idWorkspace, DateTime date)
    {
        var reservations = GetWorkSpaceReservations(idWorkspace, new DateOnly(date.Year, date.Month, date.Day));
        var objects = GetWorkSpaceObjects(idWorkspace);
        return CheckReservations(start, end, reservations, objects);
    }

    public static async Task<int> Reserve(int idWorkspace, IFormCollection form)
    {
        var dictionary = new Dictionary<string, object>()
        {
            { "idObject", int.Parse(form["idObject"]) },
            { "idUser", int.Parse(form["idUser"]) },
            { "idWorkspace", idWorkspace},
            { "timeStart", form["timeStart"].ToString() },
            { "timeEnd", form["timeEnd"].ToString() },
            { "date", form["date"].ToString() }
        };

        var responseMessage = await HttpRequestSender.SendPostRequest(dictionary, "https://localhost:7077/reserve");
        return responseMessage.StatusCode != HttpStatusCode.OK ? 0 : int.Parse(form["idObject"]);
    }
}