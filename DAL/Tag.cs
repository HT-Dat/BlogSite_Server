using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;
[Table("Tag")]
public class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id", TypeName = "int")]
    public int Id { get; set; }
    
    [Column("name", TypeName = "nvarchar(max)")]
    public string Name { get; set; }

    public ICollection<PostTag> PostTags { get; set; }
}