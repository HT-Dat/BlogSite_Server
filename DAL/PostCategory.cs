using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;

public class PostCategory
{
    [ForeignKey("Category")]
    [Column("category_id", TypeName = "int")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    [ForeignKey("Post")]
    [Column("post_id",TypeName = "int")]
    public int PostId { get; set; }
    public Post Post { get; set; }
}