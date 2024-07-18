using AutoMapper;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Helpers;

public class MappingProfiles : Profile
{
    private readonly IUserRepository _userRepository;
    private readonly IWorkspaceRepository _workspaceRepository;

    public MappingProfiles(IUserRepository userRepository, IWorkspaceRepository workspaceRepository)
    {
        _userRepository = userRepository;
        _workspaceRepository = workspaceRepository;
        CreateMap<Review, ReviewDto>()
            .BeforeMap((src, dest)
                => dest.UserName = userRepository.FindByCondition(x => x.Id == src.IdUser).First().Login);

        CreateMap<Image, WorkspaceImage>();
        CreateMap<Image, AmenityImage>();
        CreateMap<Image, ObjectImage>();
        CreateMap<ModifyWorkspaceDto, Workspace>()
            .ForMember(dest => dest.Images, opt => opt.Ignore());
        CreateMap<BaseInfo, Workspace>().ReverseMap();
        CreateMap<VerificationCode, VerificationCodeDto>()
            .BeforeMap((src, dest)
                => dest.IdCreator = workspaceRepository.FindByCondition(x => x.Id == src.IdWorkspace).First().IdCreator)
            .BeforeMap((src, dest)
                => dest.WorkspaceName = workspaceRepository.FindByCondition(x => x.Id == src.IdWorkspace).First().Name);
    }
}