using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Controllers;



public class AuthenticationController : Controller
{
    private readonly ILogger<AuthenticationController> Logger;
    private IConfiguration Configuration;
    private IHttpContextAccessor HttpContextAccessor;
    private IAuthenticationService AuthenticationService;

   public AuthenticationController(ILogger<AuthenticationController> logger,IAuthenticationService authenticationService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
   {
       Logger = logger;
       AuthenticationService = authenticationService;
       HttpContextAccessor = httpContextAccessor;
       Configuration = configuration;
   }
    
    [Route("/log-out")]
    [HttpPost]
    public void ClearJwtToken()
    {
        HttpContextAccessor.HttpContext.Session.Clear();
    }
    
    [HttpPost]
    public async Task<IActionResult> SendCode(IFormCollection form)
    {
        var code = await AuthenticationService.SendEmailAsync(form["email"].ToString(), "Администация Сайта");
        return int.TryParse(code, out var number) ? Ok(number) : Problem(code);
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(IFormCollection form)
    {
        if (form["code"].ToString() != form["correctCode"])
        {
            return BadRequest("Неправильный Код");
        }
        var response = await AuthenticationService.Register(form);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode((int)response.StatusCode, response.Message);
        }
        HttpContextAccessor.HttpContext.Session.SetString("JwtToken", response.Token);
        return Ok(response.Token);
    }
    
    
    
    [HttpPost]
    public async Task<IActionResult> Login(IFormCollection form)
    {
        var response = await AuthenticationService.Login(form);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode((int)response.StatusCode, response.Message);
        }
        HttpContextAccessor.HttpContext.Session.SetString("JwtToken", response.Token);
        return Ok(response.Token);
    }
}