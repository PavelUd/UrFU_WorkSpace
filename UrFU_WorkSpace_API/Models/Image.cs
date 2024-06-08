using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UrFU_WorkSpace_API.Enums;

namespace UrFU_WorkSpace_API.Models;

[Table("workspace_images")]
public class Image
{
    [Column("image_id")] 
    public int Id { get; set; }

    [Column("url")]
    public string Url { get; set; }
    
    [Column("owner_id")]
    public int IdOwner { get; set; }
    
}