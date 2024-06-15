namespace UrFU_WorkSpace.Models;

public class VerificationCode
{
    public int Id { get; set; }
    
    public string Code { get; set; }
    public int IdWorkspace { get; set; }
    public string WorkspaceName { get; set; } 
    
    public int IdCreator { get; set; } 
    
    public DateOnly Date { get; set; }
}