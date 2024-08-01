using AutoMapper;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ModifyUserDto, User>();
        CreateMap<Review, ReviewDto>();
        CreateMap<Image, WorkspaceImage>();
        CreateMap<Image, TemplateImage>();
        CreateMap<ModifyTemplateDto, Template>().ForMember(dest => dest.Image, opt => opt.Ignore());
        CreateMap<ModifyWorkspaceDto, Workspace>().ForMember(dest => dest.Images, opt => opt.Ignore());
    }
}