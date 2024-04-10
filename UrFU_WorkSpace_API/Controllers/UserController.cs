using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : Controller
{
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
        
        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetUser(int userId)
        {
            var user = _userRepository.GetUser(userId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        
        [HttpPost("register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] User userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            var user = _userRepository
                .GetAllUsers()
                .FirstOrDefault(user => user.Email.Trim().ToUpper() == userCreate.Email.TrimEnd().ToUpper() 
                          || user.LoginText.Trim().ToUpper() == userCreate.LoginText.TrimEnd().ToUpper());

            if(user != null)
            {
                ModelState.AddModelError("", "Такой пользователь уже существует");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_userRepository.CreateUser(userCreate))
            {
                ModelState.AddModelError("", "Что-то пошло не так");
                return StatusCode(500, ModelState);
            }

            return Ok("Пользователь успешно зарегестрирован");
        }
}