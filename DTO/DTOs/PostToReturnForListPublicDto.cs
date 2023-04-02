using DAL.Entities;

namespace DTO.DTOs;

public class PostToReturnForListPublicDto
{
    public string Title { get; set; }
    public UserInPostToReturnDto Author { get; set; }
    public DateTime? PublishedDate { get; set; }
    public String Preview { get; set; }
    public string Permalink { get; set; }
    public string ThumbnailUrl { get; set; }
}