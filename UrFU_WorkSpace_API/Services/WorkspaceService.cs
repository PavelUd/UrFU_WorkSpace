using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services;

public class WorkspaceService
{
    private readonly IWorkspaceRepository WorkspaceRepository;
    private readonly IWorkspaceComponentService<WorkspaceAmenity> AmenityService;
    private readonly IWorkspaceComponentService<WorkspaceObject> ObjectService;
    private readonly IWorkspaceComponentService<WorkspaceWeekday> OperationModeService;
    private readonly ImageService ImageService;
    public IMapper Mapper { get; set; }
    
    public WorkspaceService(IWorkspaceRepository workspaceRepository, 
        IWorkspaceComponentService<WorkspaceAmenity> amenityRepository, 
        IWorkspaceComponentService<WorkspaceObject> objectRepository, 
        IWorkspaceComponentService<WorkspaceWeekday> operationModeRepository, ImageService imageService,
        IMapper mapper)
    {
        WorkspaceRepository = workspaceRepository;
        AmenityService = amenityRepository;
        ObjectService = objectRepository;
        OperationModeService = operationModeRepository;
        ImageService = imageService;
        Mapper = mapper;
    }

    public Result<IEnumerable<Workspace>> GetWorkspaces(int? idUser)
    {
        var workspaces = WorkspaceRepository.FindAll();
        if (idUser != null)
        {
            workspaces = workspaces.Where(x => x.IdCreator == idUser);
        }
        
        return !workspaces.Any() ? Result.Fail<IEnumerable<Workspace>>(ErrorHandler.RenderError(ErrorType.WorkspacesNotFound)) 
            : Result.Ok<IEnumerable<Workspace>>(workspaces);
    }

    public Result<Workspace> GetWorkspaceById(int idWorkspace)
    {
        var workspace = WorkspaceRepository.FindByCondition(x => x.Id == idWorkspace).FirstOrDefault();
        var arg = new Dictionary<string, string>
        {
            { "idWorkspace", idWorkspace.ToString() }
        };
        
        return workspace != null
            ? Result.Ok(workspace)
            : Result.Fail<Workspace>(ErrorHandler.RenderError(ErrorType.WorkspaceNotFound, arg));
    }
    
    public Result<IEnumerable<WorkspaceWeekday>> GetOperationMode(int idWorkspace)
    {

        return OperationModeService.GetComponents(idWorkspace);
    }

    public Result<IEnumerable<WorkspaceAmenity>> GetAmenities(int idWorkspace)
    {
        return  AmenityService.GetComponents(idWorkspace);
    }

    public Result<IEnumerable<WorkspaceObject>> GetObjects(int idWorkspace)
    {
        return  ObjectService.GetComponents(idWorkspace);
    }

    public Result<int> CreateWorkspace(ModifyWorkspaceDto modifyWorkspace)
    {
        return ValidateWorkspaceComponents(modifyWorkspace)
            .Then(_ =>ConstructWorkspace(modifyWorkspace))
            .Then(WorkspaceRepository.Create);
    }

    private Result<None> ValidateWorkspaceComponents(ModifyWorkspaceDto workspace)
    {
        return ObjectService.ValidateComponents(workspace.Objects)
            .Then(_ => OperationModeService.ValidateComponents(workspace.OperationMode))
            .Then(_ => AmenityService.ValidateComponents(workspace.Amenities));
    }

    private Result<Workspace> ConstructWorkspace(ModifyWorkspaceDto modifyWorkspace, int id = 0)
    {
        var images = ImageService.ConstructImages(modifyWorkspace.ImageFiles);
        var workspace = Mapper.Map<Workspace>(modifyWorkspace);
        workspace.Images = images.Select(x => Mapper.Map<WorkspaceImage>(x)).ToList().AsEnumerable();
        workspace.Id = id;
        
        return Result.Ok(workspace);
    }
    
    public Result<Workspace> PutWorkspace(ModifyWorkspaceDto modifyWorkspace, int idWorkspace)
    {

        var oldWorkspaceResult = GetWorkspaceById(idWorkspace);

        if (!oldWorkspaceResult .IsSuccess)
        {
            return Result.Fail<Workspace>(oldWorkspaceResult.Error);
        }
        
        return ValidateWorkspaceComponents(modifyWorkspace)
            .Then(_ => ConstructWorkspace(modifyWorkspace, idWorkspace))
            .Then(x => WorkspaceRepository.Replace(oldWorkspaceResult.Value, x));
    }



    public Result<None> DeleteWorkspace(int id)
    { 
        return GetWorkspaceById(id).Then(WorkspaceRepository.Delete);
    }
    
    public Result<None> UpdateBaseInfo(int idWorkspace, JsonPatchDocument<BaseInfo> workspaceComponent)
    {
        return GetWorkspaceById(idWorkspace)
            .Then(x => Mapper.Map<BaseInfo>(x))
            .Then(b =>
            {
                workspaceComponent.ApplyTo(b);
                var workspace= Mapper.Map<Workspace>(b);
                workspace.Id = idWorkspace;
                return workspace;
            })
            .Then(WorkspaceRepository.Update);
    }
    
    private static Result<IEnumerable<T>> GetWorkspaceComponent<T>(IBaseRepository<T> repository,int idWorkspace) where T : IWorkspaceComponent
    {
        var message = ErrorHandler.RenderError(ErrorType.WorkspaceComponentNotFound);
        var entities = repository.FindByCondition(x => x.IdWorkspace == idWorkspace);
        return !entities.Any() ?
            Result.Fail<IEnumerable<T>>(message) 
            : Result.Ok(entities.AsEnumerable());
    }
    
    
    
    
}