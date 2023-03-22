namespace DTO.DTOs;

public class PostToAddDto
{
    public string? AuthorId { get; set; }
    public int? ParentId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }

}