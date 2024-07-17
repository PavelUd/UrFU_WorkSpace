using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UrFU_WorkSpace_API.Interfaces;

namespace UrFU_WorkSpace_API.Models;

[Table("object_templates")]
    public class ObjectTemplate : IModel
    {
        [Key] [Column("template_id")] public int Id { get; set; }

        public ObjectImage Image;

        [Column("category")] public string Category { get; set; }

        [Column("isReservable")] public bool IsReservable { get; set; }
        
    }