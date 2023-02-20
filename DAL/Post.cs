using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;

public class Post
{
    public Post()
    {
        ChildPosts = new HashSet<Post>();
    }

    [Key]
    [Column("id",TypeName = "int")]
    public int Id { get; set; }

    [ForeignKey("Author")]
    [Column("author_id",TypeName = "int")]
    public int AuthorId { get; set; }
    public User Author { get; set; }
    
    [ForeignKey("Parent")]
    [Column("parent_id", TypeName = "int")]
    public int? ParentId { get; set; }
    public Post ParentPost { get; set; }
    
    [Column("created_date", TypeName = "datetime2(7)")]
    public DateTime CreatedDate { get; set; }
    
    [Column("updated_date", TypeName = "datetime2(7)")]
    public DateTime UpdatedDate { get; set; }
    
    [Column("published_date", TypeName = "datetime2(7)")]
    public DateTime PublishedDate { get; set; }
    
    
    
    public virtual ICollection<Post> ChildPosts { get; set; }
    
}