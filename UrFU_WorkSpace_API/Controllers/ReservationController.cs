using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services;

namespace UrFU_WorkSpace_API.Controllers;

[Tags("Бронирование")]
[Route("api/reservations")]
[Authorize(Roles = nameof(Role.Admin) + "," + nameof(Role.User))]
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

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reservation>))]
    public IActionResult GetReservations(int idUser, int idWorkspace, DateTime date)
    {
        var result = _reservationService.GetReservations(x => (x.IdWorkspace == idWorkspace || idWorkspace == 0) 
                                                              && (x.IdUser == idUser || idUser == 0)
                                                              && date == DateTime.MinValue || x.Date == new DateOnly(date.Year, date.Month, date.Day));
        return !result.IsSuccess
            ? StatusCode((int)result.Error.HttpStatusCode, result.Error)
            : Ok(result.Value);
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
    { var idUser = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")!.Value);
        var result = _reservationService.DeleteReservation(id, idUser);
        return !result.IsSuccess
            ? StatusCode((int)result.Error.HttpStatusCode, result.Error)
            : NoContent();
    }
}