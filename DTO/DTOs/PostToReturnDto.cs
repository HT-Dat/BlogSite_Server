namespace DTO.DTOs;

public class PostToReturnDto
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public DateTime PublishedDate { get; set; }
    public byte? StatusId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Permalink { get; set; }
}