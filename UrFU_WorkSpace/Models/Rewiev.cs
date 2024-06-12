using UrFU_WorkSpace.Services;

namespace UrFU_WorkSpace.Models;

public class Review
{
    public int Id { set; get; }
    
    public int IdUser { set; get; }
    
    public int IdWorkspace { set; get; }
    
    public string Message { set; get; }
    
    public double Estimation { set; get; }

    public DateOnly Date { set; get; }
}