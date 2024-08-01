namespace UrFU_WorkSpace.Models;

public class Error
{
    public Error(string message = "", string type = "")
    {
        Message = message;
        Type = type;
    }

    public int Code;
    
    public string Type { get; set; }
    
    public string Message { get; set; }

    public Error? SubError { get; set; }
}