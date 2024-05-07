using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrFU_WorkSpace_API.Models;

[Table("workspaces")]
public class Workspace
{
    [Key] [Column("workspace_id")] public int IdWorkspace { get; set; }
    
    [Required] [Column("name")] public string Name { get; set; }

    [Required] [Column("description")] public string Description { get; set; }

    [Column("rating")] public double Rating { get; set; }

    [Column("institute")] public string Institute { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("address")]
    public string Address { get; set; }

    [Required] [Column("privacy")] public int Privacy { get; set; }

    [Required] [Column("creator_id")] public int IdCreator { get; set; }
}