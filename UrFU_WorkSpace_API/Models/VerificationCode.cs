using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UrFU_WorkSpace_API.Helpers;

namespace UrFU_WorkSpace_API.Models;

[Table("verification_code")] 
public class VerificationCode: IModel
{
    [Key][Column("id_code")]
    public int Id { get; set; }
    
    [Column("code")]
    public string Code { get; set; }
    
    [Column("id_workspace")]
    public int IdWorkspace { get; set; } 
    
    
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    [Column("date")]
    public DateOnly Date { get; set; }
    
}