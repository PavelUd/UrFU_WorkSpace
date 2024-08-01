using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IAmenityService
{
    public List<WorkspaceAmenity> ConstructWorkspaceAmenities(List<int> idTemplates);
}