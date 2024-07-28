using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IWorkspaceService
{
    public Workspace GetWorkspace(int idWorkspace);

    public Workspace ConstructWorkspace(Dictionary<string, object> baseInfo, IEnumerable<WorkspaceAmenity> amenities,
        IEnumerable<WorkspaceObject> objects, IEnumerable<WorkspaceWeekday> operationMode, IEnumerable<Image> images,
        int idUser);

    public int UpdateWorkspaceRating(int idWorkspace, double rating);
    
    public int CreateWorkspace(int idUser, Dictionary<string, object> baseInfo,
        List<(string, string)> operationModeJson, List<int> idTemplates, string jsonObjects,
        IFormFileCollection uploads,
        IWebHostEnvironment appEnvironment);

    public List<TimeSlot> GetWorkspaceTimeSlots(int idWorkspace, DateTime date, TimeType timeType,int idTemplate);

    public List<WorkspaceObject> GetReservedObjects(TimeOnly start, TimeOnly end, int idWorkspace, DateTime date,int idTemplate);
}