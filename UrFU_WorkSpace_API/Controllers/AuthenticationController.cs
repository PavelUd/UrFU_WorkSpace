using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services;

namespace UrFU_WorkSpace_API.Controllers;

[Tags("Авторизация")]
[Route("api/oauth")]
public class AuthenticationController : Controller
{
    private readonly AuthenticationService AuthenticationService;
    public IMapper Mapper { get; set; }
    public AuthenticationController(AuthenticationService authenticationService, IMapper mapper)
    {
        AuthenticationService = authenticationService;
        Mapper = mapper;
    }
    
     /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <remarks>
        /// Этот метод регистрирует нового пользователя, отправляет на указанный адрес электронной почты код подтверждения для активации аккаунта.&#xA;
        /// <code>Важно!</code>Код подтверждения действителен только в течение определенного времени. Если отправка кода подтверждения не удалась, регистрация не будет завершена.&#xA;
        /// </remarks>
        /// <param name="modifyUser">
        /// Объект, содержащий данные пользователя для регистрации.
        /// </param>
        /// <response code="202">Регистрация прошла успешно и код подтверждения отправлен на email.</response>
        /// <response code="409">Произошла ошибка конфликта данных при регистрации пользователя </response>
        /// <response code="500">Произошла ошибка сервера при регистрации пользователя или отправке кода подтверждения.</response>
        [HttpPost("register")]
        [ProducesResponseType(202)]
        [ProducesResponseType(500, Type = typeof(Error))]
        [ProducesResponseType(409, Type = typeof(Error))]
        public IActionResult Register([FromBody] ModifyUserDto modifyUser)
        {
            var user = Mapper.Map<User>(modifyUser); 
            
          var result = AuthenticationService.Register(user)
              .Then(code => AuthenticationService.SendConfirmEmail(code, user.Email));

          
          if (!result.IsSuccess)
          {
              var error = result.Error;
              return StatusCode((int)error.Status, error);
          }

          return Accepted();
        }
        
        /// <summary>
        /// Поличть токен авторизации.
        /// </summary>
        /// <remarks>
        /// Этот метод используется для авторизации пользователя.&#xA;
        /// Есть два типа гаранта для получения токена:
        /// - <c>code</c> достаточно ввести код с почты
        /// - <c>password</c> нужно ввести пароль и логин 
        /// </remarks>
        /// <returns>
        /// Возвращает результат аутентификации. При успешной авторизации возвращает статус 200 (OK) и Jwt токен.
        /// При ошибке возвращает соответствующий статус ошибки и сообщение.
        /// </returns>
        /// <response code="200">При успешной аутентификации возвращает статус 200 (OK) и Jwt токен.</response>
        /// <response code="500">Произошла ошибка сервера при аутентификации пользователя</response>
        [HttpPost("token")]
        [ProducesResponseType(200, Type = typeof(JWTToken))]
        public IActionResult GetAuthenticationToken([FromBody] TokenRequest model)
        {
            var result = AuthenticationService.GetAccessToken(model);

            return !result.IsSuccess 
                ? StatusCode((int)result.Error.Status, result.Error) 
                : Ok(new JWTToken(result.Value));
        }
}