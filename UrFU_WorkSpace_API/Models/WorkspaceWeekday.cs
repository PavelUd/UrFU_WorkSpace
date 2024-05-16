using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UrFU_WorkSpace_API.Helpers;

namespace UrFU_WorkSpace_API.Models;

[Table("workspace_operation_mode")]
public class WorkspaceWeekday
{
    [Key] 
    [Column("weekday_id")] 
    public int Id { get; set; }

    [Column("workspace_id")] 
    public int IdWorkspace { get; set; }
    
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    [Column("time_start")]
    public TimeOnly TimeStart { get; set; }
        
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    [Column("time_end")] 
    public TimeOnly TimeEnd { get; set; }
    
    [Column("day_of_week")] 
    public int WeekDayNumber { get; set; }

}