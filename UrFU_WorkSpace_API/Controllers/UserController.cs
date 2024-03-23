using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Repository;

namespace UrFU_WorkSpace_API.Controllers;

[Route("api/Users")]
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
        
        [HttpGet("{UserId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetUser(int UserId)
        {
            var user = _userRepository.GetUser(UserId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }
}