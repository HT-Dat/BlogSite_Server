using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;

public class Comment
{
    [Key]
    [Column("id", TypeName = "int")]
    public int Id { get; set; }
    
    [ForeignKey("Author")]
    [Column("author_id",TypeName = "int")]
    public int AuthorId { get; set; }
    public User Author { get; set; }
    
    [ForeignKey("ParentComment")]
    [Column(nameof(ParentComment), TypeName = "int")]
    public int? ParentId { get; set; }
    public Post ParentComment { get; set; }
    
    [Column("posted_date",TypeName = "datetime2(7)")]
    public DateTime PostedDate { get; set; }
    
    [Column("content", TypeName = "nvarchar(max)")]
    public string Content { get; set; }

    [ForeignKey(nameof(Post))]
    [Column("post_id",TypeName = "int")]
    public int PostId { get; set; }
    public Post Post { get; set; }
}