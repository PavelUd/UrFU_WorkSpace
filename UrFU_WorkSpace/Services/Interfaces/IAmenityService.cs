using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IAmenityService
{
    public List<WorkspaceAmenity> ConstructWorkspaceAmenities(List<int> idTemplates);

    public Task<List<WorkspaceAmenity>> GetWorkspaceAmenities(int idWorkspace);
    public Task<IEnumerable<AmenityTemplate>> GetAmenityTemplates();
}