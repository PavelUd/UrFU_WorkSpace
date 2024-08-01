using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Dto;

public class ReviewDto(Review review, string userName)
{
    public int Id { set; get; } = review.Id;

    public int IdUser{ set; get; } = review.IdUser;
    public string UserName { set; get; } = userName;

    public int IdWorkspace { set; get; } = review.IdWorkspace;

    public string Message { set; get; } = review.Message;

    public double Estimation { set; get; } = review.Estimation;
    public DateOnly Date { set; get; } = review.Date;
}