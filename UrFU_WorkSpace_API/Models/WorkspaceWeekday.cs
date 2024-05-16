using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrFU_WorkSpace_API.Models;

[Table("workspace_operation_mode")]
public class WorkspaceWeekday
{
    [Key] [Column("weekday_id")] public int Id { get; set; }

    [Column("workspace_id")] public int WorkspaceId { get; set; }

}