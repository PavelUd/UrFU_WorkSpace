using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Repository.Interfaces;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Services;

public class ReviewService
{
    private readonly ErrorHandler _errorHandler;
    private readonly IBaseRepository<Review> _repository;

    public ReviewService(IBaseRepository<Review> repository, ErrorHandler errorHandler)
    {
        _repository = repository;
        _errorHandler = errorHandler;
    }

    public IEnumerable<Review> GetReviews(int idWorkspace)
    {
        var reviews = _repository.FindAll();
        if (idWorkspace != 0)
            reviews = reviews.Where(x => x.IdWorkspace == idWorkspace);

        return reviews;
    }

    public Result<int> AddReview(Review review)
    {
        return Result.Ok(review).Then(_repository.Create);
    }
}