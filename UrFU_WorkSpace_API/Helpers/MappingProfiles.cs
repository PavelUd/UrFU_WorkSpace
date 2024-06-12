using AutoMapper;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Helpers;

public class MappingProfiles : Profile
{
    private readonly IUserRepository _userRepository;

    public MappingProfiles(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        CreateMap<Review, ReviewDto>()
            .BeforeMap((src, dest)
                => dest.UserName = userRepository.FindByCondition(x => x.Id == src.IdUser).First().Login);
    }
}