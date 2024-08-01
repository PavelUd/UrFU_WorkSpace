using System.ComponentModel.DataAnnotations.Schema;
using UrFU_WorkSpace_API.Interfaces;

namespace UrFU_WorkSpace_API.Models;

[Table("reviews")]
public class Review : IWorkspaceComponent
{
    [Column("user_id")] public int IdUser{ set; get; }

    [Column("message")] public string Message { set; get; }

    [Column("estimation")] public double Estimation { set; get; }

    [Column("date")] public DateOnly Date { set; get; }

    [Column("review_id")] public int Id { set; get; }

    [Column("workspace_id")] public int IdWorkspace { set; get; }
}