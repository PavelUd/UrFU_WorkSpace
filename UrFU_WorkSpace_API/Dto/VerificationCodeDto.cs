namespace UrFU_WorkSpace_API.Dto;

public class VerificationCodeDto
{
    public int Id { get; set; }
    
    public string Code { get; set; }
    public int IdWorkspace { get; set; }
    public string WorkspaceName { get; set; } 
    
    public int IdCreator { get; set; } 
    
    public DateOnly Date { get; set; }
}