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

        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <remarks>
        /// Этот метод регистрирует нового пользователя, отправляет на указанный адрес электронной почты код подтверждения для активации аккаунта.&#xA;
        /// <code>Важно!</code>Код подтверждения действителен только в течение определенного времени. Если отправка кода подтверждения не удалась, регистрация не будет завершена.&#xA;
        /// Возвращает Jwt токен.
        /// </remarks>
        /// <param name="modifyUser">
        /// Объект, содержащий данные пользователя для регистрации.
        /// </param>
        /// <response code="200">Регистрация прошла успешно и код подтверждения отправлен на email. Возвращает Jwt токен</response>
        /// <response code="409">Произошла ошибка конфликта данных при регистрации пользователя </response>
        /// <response code="500">Произошла ошибка сервера при регистрации пользователя или отправке кода подтверждения.</response>
        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(Error))]
        [ProducesResponseType(409, Type = typeof(Error))]
        public IActionResult Register([FromBody] ModifyUserDto modifyUser)
        {
            var user = Mapper.Map<User>(modifyUser); 
          var registerResult = UserService.Register(user);
          if (!registerResult.IsSuccess)
          {
              var error = registerResult.Error;
              return StatusCode((int)error.Status, error);
          }

          var code = Utils.GetConfirmCode();
          var emailSendResult = UserService.SendConfirmEmail(code, user.Email)
              .Then(_ => UserService.SaveUserInfo(user, code));
          
          if (!emailSendResult.IsSuccess)
          {
              var error = emailSendResult.Error;
              return StatusCode((int)error.Status, error);
          }
          
          return Ok(registerResult.Value);
        }
        
        /// <summary>
        /// Вход пользователя.
        /// </summary>
        /// <remarks>
        /// Этот метод используется для аутентификации пользователя. 
        /// </remarks>
        /// <returns>
        /// Возвращает результат аутентификации. При успешной аутентификации возвращает статус 200 (OK) и Jwt токен.
        /// При ошибке возвращает соответствующий статус ошибки и сообщение.
        /// </returns>
        /// <response code="200">При успешной аутентификации возвращает статус 200 (OK) и Jwt токен.</response>
        /// <response code="500">Произошла ошибка сервера при аутентификации пользователя</response>
        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(Error))]
        [ProducesResponseType(404, Type = typeof(Error))]
        public IActionResult LoginUser([FromBody] AuthenticateRequest model)
        {
            var result = UserService.Authenticate(model);
            if (!result.IsSuccess)
            {
                var error = result.Error;
                return StatusCode((int)error.Status, error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Подтвердить пользователя по почте.
        /// </summary>
        /// <remarks>
        ///  <code>Важно!</code> Код подтверждения действия 10 минут
        /// </remarks>
        /// <param name="code">
        ///    Код состоит из 8 цифр
        /// </param>
        /// <param name="idUser">id пользователя</param>
        /// <response code="200">Активация аккауна произошла успешно</response>
        /// <response code="500">Произошла ошибка сервера</response>

        [Authorize(Roles =  nameof(Role.Default))]
        [HttpPost("confirm")]
        public IActionResult Confirm([FromQuery] int code)
        {
            var userClaims = HttpContext.Items["User"] as Dictionary<string, string>;
            var result = UserService.Confirm(userClaims["Login"], code);
            if (!result.IsSuccess)
            {
                var error = result.Error;
                return StatusCode((int)error.Status, error);
            }

            return NoContent();
        }
}