using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL;
[PrimaryKey(nameof(TagId), nameof(PostId))]
public class PostTag
{
    [ForeignKey("Tag")]
    [Column("tag_id", TypeName = "int")]
    public int TagId { get; set; }
    public Tag Tag { get; set; }
    
    [ForeignKey("Post")]
    [Column("post_id",TypeName = "int")]
    public int PostId { get; set; }

    public Post Post { get; set; }
}