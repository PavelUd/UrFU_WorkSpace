using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UrFU_WorkSpace_API.Helpers;

namespace UrFU_WorkSpace_API.Models;

[Table("workspace_reservations")]
public class Reservation : IModel
{
    [Column("object_id")] public int IdObject { get; set; }

    [Column("user_id")] public int IdUser { get; set; }

    [Column("workspace_id")] public int IdWorkspace { get; set; }

    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    [Column("time_start")]
    public TimeOnly TimeStart { get; set; }

    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    [Column("time_end")]
    public TimeOnly TimeEnd { get; set; }

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("isConfirmed")] public bool IsConfirmed { get; set; }

    [Key] [Column("reservation_id")] public int Id { get; set; }
}