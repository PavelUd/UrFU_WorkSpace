using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers;

[Route("api/reservations")]
[ApiController]
public class ReservationController : Controller
{
    private readonly IReservationRepository reservationRepository;
    private readonly IMapper mapper;

    public ReservationController(IReservationRepository reservationRepository, IMapper mapper)
    {
        this.reservationRepository = reservationRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reservation>))]
    public IActionResult GetReservations(int? idUser, int? idWorkspace, DateTime date)
    {
        IEnumerable<Reservation> reservations = reservationRepository.FindByCondition(r => r.Date == date.Date);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(GetFilteredReservations(idUser, idWorkspace, reservations));
    }
    
    [HttpPost("/reserve")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reservation>))]
    public IActionResult GetReservations([FromBody] Reservation reservation)
    {
        var options = new JsonSerializerOptions() { WriteIndented = true };
        reservationRepository.Create(reservation);
        var isSaved = reservationRepository.Save();

        if (!isSaved)
            return BadRequest(ModelState);

        return Ok(reservation);
    }
 
    private static IEnumerable<Reservation> GetFilteredReservations(int? idUser, int? idWorkspace,
        IEnumerable<Reservation> reservations)
    {
        return (idUser, idWorkspace) switch
        {
            (null, not null) => reservations.Where(x => x.IdWorkspace == idWorkspace),
            (not null, not null) => reservations.Where(x => x.IdWorkspace == idWorkspace && x.IdUser == idUser),
            (not null, null) => reservations.Where(x => x.IdUser == idUser),
            _ => reservations
        };
    }
}