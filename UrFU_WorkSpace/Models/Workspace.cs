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
        var responseMessage = HttpRequestSender.SendRequest(baseAdress + $"/{idWorkspace}", RequestMethod.Get).Result;
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
        var operationModeMessage = HttpRequestSender.SendRequest(baseAdress + $"/{idWorkspace}/objects", RequestMethod.Get).Result;
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
        var operationModeMessage = HttpRequestSender.SendRequest(route, RequestMethod.Get).Result;
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
        var operationModeMessage = HttpRequestSender.SendRequest(baseAdress + $"/{idWorkspace}/operation-mode", RequestMethod.Get).Result;
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

        var responseMessage = await HttpRequestSender.SendRequest("https://localhost:7077/reserve", RequestMethod.Post, dictionary);
        return responseMessage.StatusCode != HttpStatusCode.OK ? 0 : int.Parse(form["idObject"]);
    }

    public static int CreateWorkspace(IFormCollection form, int idUser)
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
        var message = HttpRequestSender.SendRequest(baseAdress + "/add-workspace", RequestMethod.Put, baseInfo).Result;
        if (!message.IsSuccessStatusCode)
        {
            return 0;
        }

        var idWorkspace = message.Content.ReadAsStringAsync().Result;
        return int.Parse(idWorkspace);
    }
    public static bool CreateOperationMode(IFormCollection form, int idWorkspace)
    {
        var operationMode = new List<(string, string)>()
        {
            (form["mondayStart"], form["mondayEnd"]),
            (form["tuesdayStart"], form["tuesdayEnd"]),
            (form["wednesdayStart"], form["wednesdayEnd"]),
            (form["thursdayStart"], form["thursdayEnd"]),
            (form["fridayStart"], form["fridayEnd"]),
            (form["saturdayStart"], form["saturdayEnd"]),
            (form["sundayStart"], form["sundayEnd"]),
        };
        var flag = true;
        for (var i = 0; i < operationMode.Count; i++)
        {
            if (operationMode[i].Item1 == "" || operationMode[i].Item2 == "")
                continue;
            var dayOfWeek = i + 1;
            var dictionary = new Dictionary<string, object>()
            {
                {"idWorkspace", idWorkspace},
                {"timeStart", operationMode[i].Item1},
                {"timeEnd", operationMode[i].Item2},
                {"weekDayNumber", dayOfWeek}
            };
            
            var message = HttpRequestSender.SendRequest(baseAdress + "/add-weekday", RequestMethod.Put, dictionary).Result;
            if (!message.IsSuccessStatusCode)
            {
                return false;
            }
        }

        return flag;

    }

    public static List<string> SaveImages(IWebHostEnvironment appEnvironment, IFormFileCollection images)
    {
        var urls = new List<string>();
        foreach(var uploadedFile in images)
        {
            var path = "/Files/" + uploadedFile.FileName;
            using var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create);
            uploadedFile.CopyTo(fileStream);
            urls.Add(path);
        }

        return urls;
    }
    
    public static bool AddWorkspaceImages(int idWorkspace, List<string> urls)
    {
        foreach (var url in urls)
        {
            var dictionary = new Dictionary<string, object>()
            {
                { "url", url },
                { "idWorkspace", idWorkspace}
            };
            
            var message = HttpRequestSender.SendRequest(baseAdress + "/add-image", RequestMethod.Put, dictionary).Result;
            if (!message.IsSuccessStatusCode)
            {
                return false;
            }
        }

        return true;
    }
    
    public static bool CreateObjects(int idWorkspace, string jsonObjects)
    {
        var settings = new JsonSerializerSettings();
        var data = new List<Dictionary<string, object>>();
        data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonObjects, settings);

        foreach (var obj in data)
        {
            var coordinates = obj["loc"].ToString().Split(" ");
            var size = obj["size"].ToString().Split(" ");
            var objy = new Dictionary<string, object>()
            {
                { "type", obj["category"] },
                { "idWorkspace", idWorkspace },
                { "x",  coordinates[0] },
                { "y", coordinates[1] },
                { "height", size[0] },
                { "width", size[1] }
            };
            
            var message = HttpRequestSender.SendRequest(baseAdress + "/add-object", RequestMethod.Put, objy).Result;
        }
        return true;
    }
}