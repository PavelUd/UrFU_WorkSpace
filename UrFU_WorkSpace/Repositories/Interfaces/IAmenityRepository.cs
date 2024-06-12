using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Repositories.Interfaces;

public interface IAmenityRepository
{
    public bool CreateAmenity(WorkspaceAmenity data);
    public Task<IEnumerable<AmenityTemplate>> GetAmenityTemplates();
    public Task<List<WorkspaceAmenity>> GetWorkspaceAmenities(int idWorkspace);

}