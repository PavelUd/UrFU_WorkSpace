using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services.Interfaces;
using UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

namespace UrFU_WorkSpace_API.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly ImageService _imageService;
    private readonly IWorkspaceComponentService<WorkspaceAmenity> _amenityService;
    private readonly  IWorkspaceObjectService _objectService;
    private readonly IWorkspaceComponentService<WorkspaceWeekday> _operationModeService;
    private readonly TimeSlotsGenerator _timeSlotsGenerator;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IBaseProvider<Review> _reviewProvider;
    private readonly ErrorHandler _errorHandler;

    public WorkspaceService(
        IWorkspaceRepository workspaceRepository,
        IWorkspaceComponentService<WorkspaceAmenity> amenityRepository,
        IWorkspaceObjectService objectRepository,
        IWorkspaceComponentService<WorkspaceWeekday> operationModeRepository,
        ImageService imageService,
        IMapper mapper,
        ErrorHandler errorHandler, 
        IBaseProvider<Review> reviewProvider, 
        TimeSlotsGenerator timeSlotsGenerator)
    {
        _workspaceRepository = workspaceRepository;
        _amenityService = amenityRepository;
        _objectService = objectRepository;
        _operationModeService = operationModeRepository;
        _imageService = imageService;
        Mapper = mapper;
        _errorHandler = errorHandler;
        _reviewProvider = reviewProvider;
        _timeSlotsGenerator = timeSlotsGenerator;
    }

    private IMapper Mapper { get; }

    public Result<IEnumerable<Workspace>> GetWorkspaces(int? idUser, bool isFull)
    {
        var workspaces = _workspaceRepository.FindAll();
        
        if (idUser != 0)
        {
            workspaces = workspaces.Where(x => x.IdCreator == idUser);
        }

        workspaces = isFull ? _workspaceRepository.IncludeFullInfo(workspaces) : workspaces;
        return workspaces.AsEnumerable().AsResult();
    }

    public Result<Workspace> GetWorkspaceById(int idWorkspace, bool isFull)
    {
        if (!_workspaceRepository.ExistsById(idWorkspace))
        {
            return Result.Fail<Workspace>(_errorHandler.RenderError(ErrorType.WorkspaceNotFound, 
                new Dictionary<string, string>{ { "idWorkspace", idWorkspace.ToString() }}));
        }
        
        var workspaces = _workspaceRepository.FindByCondition(x => x.Id == idWorkspace);
        var workspace = isFull
            ? _workspaceRepository.IncludeFullInfo(workspaces).First()
            : workspaces.First();

        return Result.Ok(workspace);
    }

    public Result<IEnumerable<WorkspaceWeekday>> GetOperationMode(int idWorkspace)
    {
        return _operationModeService.GetComponents(idWorkspace).AsResult();
    }

    public Result<IEnumerable<WorkspaceAmenity>> GetAmenities(int idWorkspace)
    {
        return _amenityService.GetComponents(idWorkspace).AsResult();
    }

    public  Result<IEnumerable<TimeSlot>> GetTimeSlots(int idWorkspace,DateOnly dateOnly, TimeType type)
    {
        return _timeSlotsGenerator.GenerateTimeSlots(idWorkspace, dateOnly, type);
    }
    
    public Result<IEnumerable<WorkspaceObject>> GetObjects(int idWorkspace, DateOnly date, TimeOnly timeStart,
    TimeOnly timeEnd, int idTemplate)
    {
        return _objectService.GetComponents(idWorkspace,date, timeStart,timeEnd).AsResult() 
            .Then(objs => _objectService.FilterByTemplate(objs, idTemplate));
    }

    public Result<int> CreateWorkspace(ModifyWorkspaceDto modifyWorkspace)
    {
        return ValidateWorkspaceComponents(modifyWorkspace)
            .Then(_ => ConstructWorkspace(modifyWorkspace))
            .Then(_workspaceRepository.Create);
    }

    public Result<Workspace> PutWorkspace(ModifyWorkspaceDto modifyWorkspace, int idWorkspace)
    {
        if (!_workspaceRepository.ExistsById(idWorkspace))
        {
            return Result.Fail<Workspace>(_errorHandler.RenderError(ErrorType.WorkspaceNotFound, 
                new Dictionary<string, string>{ { "idWorkspace", idWorkspace.ToString() }}));
        }
        
        return ValidateWorkspaceComponents(modifyWorkspace)
            .Then(_ => ConstructWorkspace(modifyWorkspace, idWorkspace))
            .Then(x => _workspaceRepository.Replace(x));
    }

    public Result<None> DeleteWorkspace(int id)
    {
        return GetWorkspaceById(id, false).Then(_workspaceRepository.Delete);
    }
    
    public Result<None> UpdateRating(int idWorkspace)
    {
        return CalculateRating(idWorkspace).Then(r => _workspaceRepository.UpdateRating(idWorkspace, r));
    }
    
    private Result<None> ValidateWorkspaceComponents(ModifyWorkspaceDto workspace)
    {
        return _objectService.ValidateComponents(workspace.Objects)
            .Then(_ => _operationModeService.ValidateComponents(workspace.OperationMode))
            .Then(_ => _amenityService.ValidateComponents(workspace.Amenities));
    }

    private Result<Workspace> ConstructWorkspace(ModifyWorkspaceDto modifyWorkspace, int id = 0)
    {
        var images = _imageService.ConstructImages(modifyWorkspace.ImageFiles, (int)OwnerType.Workspace);
        var workspace = Mapper.Map<Workspace>(modifyWorkspace);
        workspace.Images = images.Select(x => Mapper.Map<WorkspaceImage>(x)).ToList().AsEnumerable();
        workspace.Id = id;
        workspace.Rating = CalculateRating(id).Value;
        return Result.Ok(workspace);
    }
    
    private Result<double> CalculateRating(int idWorkspace)
    {
        var reviews = _reviewProvider.FindByCondition(x => x.IdWorkspace == idWorkspace);
        var sum = reviews.Sum(x => x.Estimation);
        return !reviews.Any() ? 0 : Result.Ok().Then(_ => double.Round(sum / reviews.Count(), 1));
    } 
}