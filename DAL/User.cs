using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;
[Table("User")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column("id", TypeName = "varchar(128)")]
    public string Id { get; set; }

    [Column("display_name", TypeName = "nvarchar(128)")]
    public string DisplayName { get; set; }

    [Column("sex_id")] 
    [ForeignKey("Sex")]
    public byte? SexId { get; set; }
    
    public Sex Sex { get; set; }

    public ICollection<Comment> Comments { get; set; }
    public ICollection<Post> Posts { get; set; }
}