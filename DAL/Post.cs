using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;

public class Post
{
    [Key]
    [Column("id",TypeName = "int")]
    public int Id { get; set; }

    [ForeignKey("Author")]
    [Column("author_id",TypeName = "int")]
    public int AuthorId { get; set; }
    public User Author { get; set; }
    
    
    
}