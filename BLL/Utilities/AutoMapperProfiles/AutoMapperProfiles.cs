using AutoMapper;
using DAL.Entities;
using DTO.DTOs;

namespace BLL.Utilities.AutoMapperProfiles;

public class AutoMapperProfiles :Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Post, PostToAddDto>().ReverseMap();
        CreateMap<Post, PostToReturnDto>().ReverseMap();
        CreateMap<Post, PostToReturnPublicDto>().ReverseMap();
        CreateMap<Post, PostToUpdateDto>().ReverseMap();
        CreateMap<Post, PostToReturnForListDto>().ReverseMap();
        CreateMap<Post, PostToReturnForListPublicDto>().ReverseMap();
        CreateMap<User, UserInPostToReturnDto>().ReverseMap();
    }
}