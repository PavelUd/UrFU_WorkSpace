using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers;

[Tags("Пользователи")]
[Route("api/users")]
[ApiController]
public class UserController : Controller
{
        private readonly IUserService UserService;
        public IMapper Mapper { get; set; }
        public UserController(IUserService userService, IMemoryCache memoryCache, IMapper mapper)
        {
            UserService = userService;
            Mapper = mapper;
        }
        
        /// <summary>
        /// Получить Список Пользователей.
        /// </summary>
        /// <response code="500">Произошла ошибка сервера</response>
        [Authorize(Roles =  nameof(Role.Admin))]
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = UserService.GetAllUsers();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }
        
        /// <summary>
        /// Получить Пользователя по id.
        /// </summary>
        /// <param name="idUser">
        /// id пользователя.
        /// </param>
        /// <response code="500">Произошла ошибка сервера</response>

        [Authorize(Roles =  nameof(Role.Admin))]
        [HttpGet("{idUser}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetUser(int idUser)
        {
            var user = UserService.GetUsersByCondition(x => x.Id == idUser).FirstOrDefault();
            if (user == null)
            {
                return NotFound("Нет такого пользователя");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }
}