using DAL.Entities;
using DTO.DTOs;

namespace BLL.Services.IServices;

public interface IPostService
{
    Task<PostToReturnDto> Get(int id);
    Task<PostToReturnPublicDto> GetPublic(string permalink);
    Task<List<PostToReturnForListDto>> GetList(string authorId);
    Task<List<PostToReturnForListPublicDto>> GetListPublic();
    
    Task<Post> Add(string authorId);
    Task Delete(int id, string authorId);
    Task<PostToReturnDto> Update(PostToUpdateDto postToUpdate, string authorId);
}