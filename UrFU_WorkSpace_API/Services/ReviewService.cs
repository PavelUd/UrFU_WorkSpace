using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Repository.Interfaces;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Services;

public class ReviewService
{
    private readonly IReviewRepository _repository;

    public ReviewService(IReviewRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<ReviewDto> GetReviews(int idWorkspace)
    {
        var reviews = _repository.FindByCondition(x => x.IdWorkspace == idWorkspace || idWorkspace == 0);
        return _repository.IncludeUserLogin(reviews);
    }

    public Result<int> AddReview(Review review)
    {
        return Result.Ok(review).Then(_repository.Create);
    }
    
}