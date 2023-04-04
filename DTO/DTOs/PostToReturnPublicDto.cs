namespace DTO.DTOs;

public class PostToReturnPublicDto
{
    public DateTime PublishedDate { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Permalink { get; set; }
    public UserInPostToReturnDto Author { get; set; }

}