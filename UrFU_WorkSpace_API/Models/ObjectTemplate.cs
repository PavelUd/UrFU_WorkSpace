using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UrFU_WorkSpace_API.Models;

[Table("object_templates")]
    public class ObjectTemplate
    {
        [Key] [Column("template_id")] public int Id { get; set; }

        [Column("picture")] public string Picture { get; set; }

        [Column("category")] public string Category { get; set; }

        [Column("isReservable")] public bool IsReservable { get; set; }
        
        [JsonIgnore]
        public ICollection<WorkspaceObject>? WorkspaceObjects { get; set; }
        
    }