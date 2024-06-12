using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;
using AmenityTemplate = UrFU_WorkSpace.Models.AmenityTemplate;

namespace UrFU_WorkSpace.Services.Interfaces;

public class AmenityService : IAmenityService
{

    private readonly IAmenityRepository Repository;

    public AmenityService(IAmenityRepository repository)
    {
        Repository = repository;
    }
    
    public List<WorkspaceAmenity> ConstructWorkspaceAmenities(List<int> idTemplates)
    {
        var amenities = new List<WorkspaceAmenity>();
        foreach (var id in idTemplates)
        {
            amenities.Add(CreateAmenity(id));
        }

        return amenities;
    }

    public async Task<IEnumerable<AmenityTemplate>> GetAmenityTemplates()
    {
        return await Repository.GetAmenityTemplates();
    }
    
    public async Task<List<WorkspaceAmenity>> GetWorkspaceAmenities(int idWorkspace)
    {
        return await Repository.GetWorkspaceAmenities(idWorkspace);
    }

    private static WorkspaceAmenity CreateAmenity(int idTemplate)
    {
        return new WorkspaceAmenity()
        {
            IdTemplate = idTemplate
        };
    }
}