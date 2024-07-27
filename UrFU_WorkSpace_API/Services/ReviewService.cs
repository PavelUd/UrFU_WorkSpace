using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Helpers.Events;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Services;

public class ReviewService : IEventHandler<WorkspaceDeletedEvent>
{
    private readonly IWorkspaceProvider _workspaceProvider;
    private readonly ErrorHandler _errorHandler;
    private readonly IBaseRepository<Review> _repository;

    public ReviewService(IBaseRepository<Review> repository, IWorkspaceProvider workspaceProvider, ErrorHandler errorHandler)
    {
        _repository = repository;
        _workspaceProvider = workspaceProvider;
        _errorHandler = errorHandler;
    }

    public IEnumerable<Review> GetReviews(int idWorkspace)
    {
        var reviews = _repository.FindAll();
        if (idWorkspace != 0)
        {
            reviews = reviews.Where(x => x.IdWorkspace == idWorkspace);
        }

        return reviews;
    }

    public Result<int> AddReview(Review review)
    {
        return _workspaceProvider.GetWorkspaceById(review.IdWorkspace, false)
            .Then(_ => _repository.Create(review));
    }
    
    public Result<None> Handle(WorkspaceDeletedEvent @event)
    {
        return  GetReviews(@event.WorkspaceId).AsResult()
            .Then(r => _repository.DeleteRange(r));
    }
}