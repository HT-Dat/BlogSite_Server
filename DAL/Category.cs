using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id", TypeName = "int")]
    public int Id { get; set; }

    [Column("name", TypeName = "nvarchar(max)")]
    public string Name { get; set; }

    [ForeignKey("Parent")]
    [Column("parent_id", TypeName = "int")]
    public int? ParentId { get; set; }

    public Category ParentCategory { get; set; }
    
    public ICollection<Category> ChildCategories { get; set; }
}