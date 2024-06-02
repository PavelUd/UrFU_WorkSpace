using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IWorkspaceService
{
    public Workspace GetWorkspace(int idWorkspace);

    public int AddBaseWorkspaceInfo(IFormCollection form, int idUser);

    public bool CreateWorkspace(int idUser, IFormCollection form, IFormFileCollection uploads,
        IWebHostEnvironment appEnvironment);

    public List<TimeSlot> GetWorkspaceTimeSlots(int idWorkspace, DateTime date, TimeType timeType, string typeObject);

    public List<WorkspaceObject> GetReservedObjects(TimeOnly start, TimeOnly end, int idWorkspace, DateTime date);
}