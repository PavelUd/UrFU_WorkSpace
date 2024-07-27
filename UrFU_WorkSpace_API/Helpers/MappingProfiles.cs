using AutoMapper;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ModifyUserDto, User>();
        CreateMap<Image, WorkspaceImage>();
        CreateMap<Image, AmenityImage>();
        CreateMap<Image, ObjectImage>();
        CreateMap<ModifyWorkspaceDto, Workspace>()
            .ForMember(dest => dest.Images, opt => opt.Ignore());
        CreateMap<BaseInfo, Workspace>().ReverseMap();
    }
}