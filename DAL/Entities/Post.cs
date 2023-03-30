using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;
[Table("Post")]

public class Post
{
    [Key]
    [Column("id",TypeName = "int")]
    public int Id { get; set; }

    [ForeignKey("Author")]
    [Column("author_id",TypeName = "varchar(128)")]
    public string AuthorId { get; set; }
    public User Author { get; set; }
    
    [ForeignKey("ParentPost")]
    [Column("parent_id", TypeName = "int")]
    public int? ParentId { get; set; }
    public Post ParentPost { get; set; }
    
    [Column("created_date", TypeName = "datetime2(7)")]
    public DateTime CreatedDate { get; set; }
    
    [Column("updated_date", TypeName = "datetime2(7)")]
    public DateTime UpdatedDate { get; set; }
    
    [Column("published_date", TypeName = "datetime2(7)")]
    public DateTime? PublishedDate { get; set; }
    
    [ForeignKey("PostStatus")]
    [Column("status_id", TypeName = "tinyint")]
    public byte? StatusId { get; set; }
    public PostStatus PostStatus { get; set; }
    
    [Column("title", TypeName = "nvarchar(255)")]
    public string Title { get; set; }
    
    [Column("content",TypeName = "nvarchar(max)")]
    public string Content { get; set; }
    
    public ICollection<Post> ChildPosts { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<PostCategory> PostCategories { get; set; }
    public ICollection<PostTag> PostTags { get; set; }
}