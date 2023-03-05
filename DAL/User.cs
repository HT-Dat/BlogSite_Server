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
    
    [Column("created_date",TypeName = "datetime2(7)")]
    public DateTime CreatedDate { get; set; }
    
    [Column("last_login",TypeName = "datetime2(7)")]
    public DateTime LastLogin { get; set; }
    
    [Column("intro",TypeName = "nvarchar(255)")]
    public string Intro { get; set; }
    
    [Column("profile",TypeName = "nvarchar(max)")]
    public string Profile { get; set; }

    public ICollection<Comment> Comments { get; set; }
    public ICollection<Post> Posts { get; set; }
}