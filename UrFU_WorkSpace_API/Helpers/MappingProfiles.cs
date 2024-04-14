using AutoMapper;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Helpers;

public class MappingProfiles : Profile
{
    private readonly IWorkspaceRepository _workspaceRepository;

    public MappingProfiles(IWorkspaceRepository workspaceRepository)
    {
        _workspaceRepository = workspaceRepository;
        CreateMap<Workspace, WorkspaceDTO>().BeforeMap((src, dest)
            => dest.Images = _workspaceRepository.GetWorkspaceImages(src.WorkspaceId));
    }
}