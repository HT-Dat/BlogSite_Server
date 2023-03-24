namespace DTO.DTOs;

public class PostToUpdate
{
    public int Id { get; set; }
    public byte? StatusId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}