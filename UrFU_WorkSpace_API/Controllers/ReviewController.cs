using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Repository;

namespace UrFU_WorkSpace_API.Controllers;

[Route("api/reviews")]
public class ReviewController : Controller
{
    private readonly IReviewRepository reviewRepository;
    private readonly IMapper mapper;

    public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
    {
        this.reviewRepository = reviewRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
    public IActionResult GetReviews(int? idUser, int? idWorkspace)
    {
        var reviews = reviewRepository.FindAll();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(GetFilteredReservations(idUser, idWorkspace, reviews).Select(x => mapper.Map<Review, ReviewDto>(x)).OrderByDescending(x => x.Date));
    }

    [HttpPost]
    [Route("add-review")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)] 
    public IActionResult AddReview([FromBody] Review review)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        reviewRepository.Create(review);
        return SaveChanges() ? CreatedAtAction(nameof(AddReview), new { id = review.Id }, review) : BadRequest();
    }

    [HttpPatch]
    [Route("update-review")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult UpdateReview(int idReview, [FromBody] UpdateReviewDto updateReviewDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var review = reviewRepository.FindByCondition(x => x.Id == idReview).FirstOrDefault();
        if (review == null)
            return NotFound();

        if (updateReviewDto.Message != null)
        {
            review.Message = updateReviewDto.Message;
        }

        if (updateReviewDto.Estimation.HasValue)
        {
            review.Estimation = updateReviewDto.Estimation.Value;
        }

        reviewRepository.Update(review);
        return SaveChanges() ? NoContent() : BadRequest();
    }

    [HttpDelete]
    [Route("delete-review")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult DeleteReview([FromBody] Review review)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        reviewRepository.Delete(review);
        return SaveChanges() ? NoContent() : BadRequest();
    }
    
    private static IEnumerable<Review> GetFilteredReservations(int? idUser, int? idWorkspace,
        IEnumerable<Review> reviews)
    {
        return (idUser, idWorkspace) switch
        {
            (null, not null) => reviews.Where(x => x.IdWorkspace == idWorkspace),
            (not null, not null) => reviews.Where(x => x.IdWorkspace == idWorkspace && x.IdUser == idUser),
            (not null, null) => reviews.Where(x => x.IdUser == idUser),
            _ => reviews
        };
    }
    
    private bool SaveChanges()
    {
        return reviewRepository.Save();
    }
}