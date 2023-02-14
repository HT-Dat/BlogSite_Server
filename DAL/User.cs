using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column("id", TypeName = "varchar(128)")]
    public string? Id { get; set; }
    
    [Column("display_name", TypeName = "nvarchar(128)")]
    public string? DisplayName { get; set; }
    
    public Sex Sex { get; set; }
}