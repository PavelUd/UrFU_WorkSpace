using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class AmenityService(IWorkspaceRepository repository) : IAmenityService
{
    public List<WorkspaceAmenity> ConstructWorkspaceAmenities(List<int> idTemplates)
    {
        var amenities = new List<WorkspaceAmenity>();
        foreach (var id in idTemplates)
        {
            amenities.Add(CreateAmenity(id));
        }

        return amenities;
    }

    private static WorkspaceAmenity CreateAmenity(int idTemplate)
    {
        return new WorkspaceAmenity()
        {
            IdTemplate = idTemplate
        };
    }
}