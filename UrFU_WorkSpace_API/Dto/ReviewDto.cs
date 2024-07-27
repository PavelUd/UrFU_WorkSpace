namespace UrFU_WorkSpace_API.Dto;

public class ReviewDto
{
    public int Id { set; get; }

    public string UserName { set; get; }

    public int IdWorkspace { set; get; }

    public string Message { set; get; }

    public double Estimation { set; get; }
    public DateOnly Date { set; get; }
}