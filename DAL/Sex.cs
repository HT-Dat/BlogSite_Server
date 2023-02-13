using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;

public class Sex
{
    [Key]
    [Column("id", TypeName = "tinyint")]
    public byte? Id { get; set; }
    
    [Column("name", TypeName = "nvarchar(64)")]
    public string? Name { get; set; }
    
    public ICollection<User> Users { get; set; }
}