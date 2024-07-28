using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers;

[Tags("Пользователи")]
[Route("api/users")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        Mapper = mapper;
    }

    public IMapper Mapper { get; set; }

    /// <summary>
    ///     Получить Список Пользователей.
    /// </summary>
    /// <response code="500">Произошла ошибка сервера</response>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    [ProducesResponseType(500)]
    [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
    public IActionResult GetUsers()
    {
        var users = _userService.GetAllUsers();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(users);
    }

    /// <summary>
    ///     Получить Пользователя по id.
    /// </summary>
    /// <param name="idUser">
    ///     id пользователя.
    /// </param>
    /// <response code="500">Произошла ошибка сервера</response>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("{idUser}")]
    [ProducesResponseType(500)]
    [ProducesResponseType(200, Type = typeof(User))]
    public IActionResult GetUser(int idUser)
    {
        var user = _userService.GetUsersByCondition(x => x.Id == idUser).FirstOrDefault();
        if (user == null) 
            return NotFound("Нет такого пользователя");
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(user);
    }
}