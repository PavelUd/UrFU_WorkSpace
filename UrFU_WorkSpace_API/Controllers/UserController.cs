using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services;

namespace UrFU_WorkSpace_API.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : Controller
{
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }
        
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetAllUsers();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }
        [HttpGet("{idUser}")]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetUser(int idUser)
        {
            var user = _userRepository.GetUser(idUser);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult RegisterUser([FromBody] User userCreate)
        {
            if(_userService.IsUserExists(userCreate))
            {
                ModelState.AddModelError("", "Такой пользователь уже существует");
                return StatusCode(422, ModelState);
            }
            
            if (userCreate == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            
            var response = _userService.Register(userCreate);
            
            if(response == null)
            {
                ModelState.AddModelError("", "Что-то пошло не так");
                return StatusCode(500, ModelState);
            }

            return Ok(response);
        }
        [HttpPost("login")]
        [ProducesResponseType(200)]
        public IActionResult LoginUser([FromBody] AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);
            
            if(response == null)
            {
                ModelState.AddModelError("", "Что-то пошло не так");
                return StatusCode(500, ModelState);
            }

            return Ok(response);
        }
}