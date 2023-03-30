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
        CreateMap<Post, PostToUpdate>().ReverseMap();
        CreateMap<Post, PostToReturnForListDto>().ReverseMap();
    }
}