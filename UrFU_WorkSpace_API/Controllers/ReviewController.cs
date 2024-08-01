using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Controllers;

[Tags("Отзывы")]
[Route("api/reviews")]
public class ReviewController : Controller
{
    private readonly ReviewService _reviewService;
    private readonly IWorkspaceService _workspaceService;

    public ReviewController(ReviewService reviewService, IWorkspaceService workspaceService)
    {
        _reviewService = reviewService;
        _workspaceService = workspaceService;
    }

    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
    public IActionResult GetReviews(int idWorkspace = 0)
    {
        var result = _reviewService.GetReviews(idWorkspace).AsResult();
        return !result.IsSuccess
            ? StatusCode((int)result.Error.HttpStatusCode, result.Error)
            : Ok(result.Value);
    }


    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [Authorize(Roles = nameof(Role.User) + "," + nameof(Role.Admin))]
    public IActionResult AddReview([FromBody] Review review)
    {
        var id  = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")!.Value);
        review.IdUser = id;
        var result = _reviewService.AddReview(review).Then(id =>
        {
            _workspaceService.UpdateRating(review.IdWorkspace);
            return id;
        });
        return !result.IsSuccess
            ? StatusCode((int)result.Error.HttpStatusCode, result.Error)
            : Ok(result.Value);
    }
}