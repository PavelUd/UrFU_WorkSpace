using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Controllers;

public class AuthenticationController(ILogger<AuthenticationController> logger, IHttpContextAccessor httpContextAccessor)
    : Controller
{
    private readonly ILogger<AuthenticationController> _logger = logger;
    
    [HttpPost]
    public async Task<IActionResult> CheckUserExistence(IFormCollection form)
    {
        var user = new User(httpContextAccessor.HttpContext);
        var isUserExist = await user.CheckUserExistence(form);
        return isUserExist ? Ok() : Ok("Такой пользователь уже есть");
    }
    
    [HttpPost]
    public async Task<IActionResult> SendCode(IFormCollection form)
    {
        var user = new User(httpContextAccessor.HttpContext);
        var code = await user.SendEmailAsync(form["email"].ToString(), "Администация Сайта");
        return int.TryParse(code, out var number) ? Ok(number) : Problem(code);
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(IFormCollection form)
    {
        if (form["code"].ToString() != form["correctCode"])
        {
            return BadRequest("Неправильный Код");
        }
        var user = new User(httpContextAccessor.HttpContext);
        await user.Register(form);
        var token = httpContextAccessor.HttpContext.Session.GetString("JwtToken");
        if (token != null)
        {
            return Ok(JwtTokenDecoder.GetUserName(token));
        }
        return BadRequest("Register failed");
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(IFormCollection form)
    {
        var user = new User(httpContextAccessor.HttpContext);
        var message = await user.Login(form);
        var token = httpContextAccessor.HttpContext.Session.GetString("JwtToken");
        if (token != null)
        {
            return Ok(JwtTokenDecoder.GetUserName(token));
        }
        if(message != "")
        {
            return BadRequest(message); 
        }
        return StatusCode(500, "Login failed");
    }
}