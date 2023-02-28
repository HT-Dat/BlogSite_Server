using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;
[Table("Category")]
public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id", TypeName = "int")]
    public int Id { get; set; }

    [Column("name", TypeName = "nvarchar(max)")]
    public string Name { get; set; }

    [ForeignKey(nameof(ParentCategory))]
    [Column("parent_id", TypeName = "int")]
    public int? ParentId { get; set; }

    public Category ParentCategory { get; set; }
    
    public ICollection<Category> ChildCategories { get; set; }
    public ICollection<PostCategory> PostCategories { get; set; }
}