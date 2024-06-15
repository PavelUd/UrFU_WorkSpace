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

    public double RecalculateRating(int idWorkspace)
    {
        var reviews = Repository.GetByIdWorkspace(idWorkspace);
        return double.Round(reviews.Select(x => x.Estimation).Sum() / reviews.Count(), 1);
    }

    public IEnumerable<Review> GetReviews(int idWorkspace)
    {
        return Repository.GetByIdWorkspace(idWorkspace);
    }

    public async Task AddReview(Review review)
    {
       await Repository.AddReview(review);
    }
}