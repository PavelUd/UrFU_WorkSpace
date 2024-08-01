using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IReviewRepository : IBaseRepository<Review>
{
    public IEnumerable<ReviewDto> IncludeUserLogin(IQueryable<Review> reviews);
}