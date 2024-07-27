using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrFU_WorkSpace_API.Models;

[Table("workspaces")]
public class Workspace : IModel
{
    [Required] [Column("name")] public string Name { get; set; }

    [Required] [Column("description")] public string Description { get; set; }

    [Column("rating")] public double Rating { get; set; }

    [Column("institute")] public string Institute { get; set; }

    public IEnumerable<WorkspaceImage> Images { get; set; }
    public IEnumerable<WorkspaceObject> Objects { get; set; }
    public IEnumerable<WorkspaceAmenity> Amenities { get; set; }
    public IEnumerable<WorkspaceWeekday> OperationMode { get; set; }

    [Column("address")] public string Address { get; set; }

    [Column("latitude")] public double Latitude { get; set; }

    [Column("longitude")] public double Longitude { get; set; }

    [Required] [Column("privacy")] public int Privacy { get; set; }

    [Required] [Column("creator_id")] public int IdCreator { get; set; }
    [Key] [Column("workspace_id")] public int Id { get; set; }
}