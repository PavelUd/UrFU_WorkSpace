using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services;

namespace UrFU_WorkSpace_API.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : Controller
{
        private readonly IUserService UserService;

        public UserController(IUserService userService)
        { 
            UserService = userService;
        }
        
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

        
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            
          var response = await UserService.Register(user);
          return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        public IActionResult LoginUser([FromBody] AuthenticateRequest model)
        {
            var response = UserService.Authenticate(model);
            return StatusCode((int)response.StatusCode, response);
        }
        
        [HttpPatch("{idUser}/update-access-level")]
        [ProducesResponseType(200)]

        public IActionResult UpdateAccessLevel(int idUser, int accessLevel)
        {
            if (!UserService.GetUsersByCondition(x => x.Id == idUser).Any())
            {
                return NotFound("Пользователь не найден");
            }
            UserService.UpdateAccessLevel(idUser, accessLevel);
            return Ok();
        }
}