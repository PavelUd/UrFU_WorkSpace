using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services;

namespace UrFU_WorkSpace_API.Controllers;

[Tags("Бронирование")]
[Authorize(Roles = nameof(Role.User) + "," +  nameof(Role.Admin))]
[Route("api/reservations")]
 
[ApiController]
public class ReservationController : Controller
{
    private readonly IMapper _mapper;
    private readonly ReservationService _reservationService;

    public ReservationController(ReservationService service, IMapper mapper)
    {
        _reservationService = service;
        this._mapper = mapper;
    }
    
    [HttpPatch("{id}/confirm")]
    public IActionResult Confirm(int id, Coordinate coordinate)
    {
        var isAdmin = HttpContext.User.IsInRole(nameof(Role.Admin));
        var idUser = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")!.Value);

        var result = isAdmin && coordinate is { Longitude: 0, Latitude: 0 }
            ? _reservationService.ConfirmReservation(id)
            : _reservationService.ConfirmReservation(id, coordinate, idUser);


        return !result.IsSuccess
            ? StatusCode((int)result.Error.HttpStatusCode, result.Error)
            : Ok(result.Value);
        ;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Reservation))]
    public IActionResult GetReservations(int id)
    {
        var result = _reservationService.GetReservationById(id);
        return !result.IsSuccess
            ? StatusCode((int)result.Error.HttpStatusCode, result.Error)
            : Ok(result.Value);
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reservation>))]
    public IActionResult GetReservations(int idUser, int idWorkspace, bool isFull = false)
    {
        var result = _reservationService.GetReservations(x => (x.IdWorkspace == idWorkspace || idWorkspace == 0) 
                                                              && (x.IdUser == idUser || idUser == 0));

        return isFull ? Ok(_reservationService.CompleteReservations(result.Value.ToList()).Value) : Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    public IActionResult AddReservation([FromBody] Reservation reservation)
    {
        var idUser = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")!.Value);
        var result = _reservationService.AddReservation(reservation, idUser);
        
        return !result.IsSuccess
            ? StatusCode((int)result.Error.HttpStatusCode, result.Error)
            : Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteReservation(int id)
    { 
        var idUser = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")!.Value);
        var result = _reservationService.DeleteReservation(id, idUser);
        
        return !result.IsSuccess
            ? StatusCode((int)result.Error.HttpStatusCode, result.Error)
            : NoContent();
    }
}