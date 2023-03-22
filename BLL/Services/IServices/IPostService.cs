using DAL.Entities;
using DTO.DTOs;

namespace BLL.Services.IServices;

public interface IPostService
{
    Task<Post> Get(int id);
    Task<Post> Add(string authorId);
    Task Delete(int id);
    Task Update(Post post);
}