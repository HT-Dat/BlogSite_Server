using DAL.Entities;
using DTO.DTOs;

namespace BLL.Services.IServices;

public interface IPostService
{
    Task<PostToReturnDto> Get(int id);
    Task<List<PostToReturnForListDto>> GetList();
    Task<List<PostToReturnForListPublicDto>> GetListPublic();
    
    Task<Post> Add(string authorId);
    Task Delete(int id);
    Task<PostToReturnDto> Update(PostToUpdateDto postToUpdate);
}