using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Repository;

[Route("api/reservations")]
[ApiController]
public class ReservationController : Controller
{
    private readonly IReservationRepository reservationRepository;
    public IMapper mapper { get; set; }
    
    public ReservationController(IReservationRepository reservationRepository, IMapper mapper)
    {
        this.reservationRepository = reservationRepository;
        this.mapper = mapper;
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reservation>))]
    public IActionResult GetUserReservations(int userId)
    { 
        var reservations = reservationRepository.GetUserReservations(userId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reservations);
    }
}