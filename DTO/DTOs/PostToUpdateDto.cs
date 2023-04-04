namespace DTO.DTOs;

public class PostToUpdateDto
{
    public int Id { get; set; }
    public byte? StatusId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Permalink { get; set; }
}