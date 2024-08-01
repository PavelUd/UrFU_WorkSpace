using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories;

namespace UrFU_WorkSpace.Services;

public class ReviewService
{
    private ReviewRepository Repository;
    
    public ReviewService(ReviewRepository repository)
    {
        Repository = repository;
    }
    

    public async Task<Result<List<Review>>> GetReviews(int idWorkspace)
    {
        return await Repository.GetAll(idWorkspace);
    }

    public async Task<Result<int>> AddReview(Review review, string token)
    {
       return await Repository.AddReview(review, token);
    }
}