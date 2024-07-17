using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    
    [HttpPatch("{id}/confirm")]
    public IActionResult Confirm(int id)
    {
        var i = reservationRepository.FindByCondition(x => x.Id == id)
            .ExecuteUpdate(b => b.SetProperty(x => x.IsConfirmed, true));

        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reservation>))]
    public IActionResult GetReservations(int? idUser, int? idWorkspace, DateTime date)
    {
        IEnumerable<Reservation> reservations = date != DateTime.MinValue 
            ? reservationRepository.FindByCondition(r => r.Date == new DateOnly(date.Year, date.Month, date.Day)) 
            : reservationRepository.FindAll();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(GetFilteredReservations(idUser, idWorkspace, reservations));
    }
    
    [HttpPost("reserve")]
    [ProducesResponseType(201, Type = typeof(IEnumerable<Reservation>))]
    public IActionResult GetReservations([FromBody] Reservation reservation)
    {
        var options = new JsonSerializerOptions() { WriteIndented = true };
        reservationRepository.Create(reservation);
        var isSaved = reservationRepository.Save();

        if (!isSaved)
            return BadRequest(ModelState);

        return Ok(reservation);
    }
    
    [HttpDelete("{id}/delete")]
    public IActionResult DeleteReservation(int id)
    {
        var reservation = reservationRepository.FindByCondition(x => x.Id == id).FirstOrDefault();
        
        if (reservation == null)
        {
            return BadRequest("Не найдено брнирование");
        }
        var now = DateTime.Now;
        var reservationTime = reservation.Date.ToDateTime(reservation.TimeStart);

        if (now >= reservationTime.AddHours(-12))
        {
            return BadRequest("Отмена брони возможна за 12 часов до забронированного времени");
        }
        
        reservationRepository.Delete(reservation);
        reservationRepository.Save();
        return Ok();
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