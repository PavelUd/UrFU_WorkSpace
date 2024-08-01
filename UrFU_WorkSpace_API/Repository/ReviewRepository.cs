using Microsoft.EntityFrameworkCore.Internal;
using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Repository;

public class ReviewRepository(UrfuWorkSpaceContext context) : BaseRepository<Review>(context), IReviewRepository
{
    public IEnumerable<ReviewDto> IncludeUserLogin(IQueryable<Review> reviews)
    {
        return reviews
            .Join(_context.Users, r => r.IdUser, u => u.Id, (r, u) => new ReviewDto(r, u.Login));
    }
    
}