using System.ComponentModel.DataAnnotations.Schema;

namespace UrFU_WorkSpace_API.Models;

[Table("reviews")]
public class Review
{
    [Column("review_id")]
    public int Id { set; get; }
    
    [Column("user_id")]
    public int IdUser { set; get; }
    
    [Column("workspace_id")]
    public int IdWorkspace { set; get; }
    
    [Column("message")]
    public string Message { set; get; }
    
    [Column("estimation")]
    public double Estimation { set; get; }
}