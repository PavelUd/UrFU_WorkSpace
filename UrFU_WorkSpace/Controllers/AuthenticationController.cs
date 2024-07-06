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
    private IUserService UserService;
    
   public AuthenticationController(ILogger<AuthenticationController> logger,IUserService userService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
   {
       Logger = logger;
       UserService = userService;
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
    public async Task<IActionResult> VerifyUser(IFormCollection form)
    {
        var idUser = int.Parse(form["idUser"]);
        var userCode = form["code"].ToString();
        var correctCode = HttpContextAccessor.HttpContext.Session.GetString("Code");
        
        var response = UserService.VerifyUser(idUser, userCode, correctCode);
        HttpContextAccessor.HttpContext.Session.Remove("Code");
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Register(IFormCollection form)
    {
        var response = await UserService.Register(form);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return Ok(response);
        }

        var code = await UserService.SendEmailAsync(form["email"].ToString(), "Администация Сайта");
        HttpContextAccessor.HttpContext.Session.SetString("JwtToken", response.Token);
        HttpContextAccessor.HttpContext.Session.SetString("Code", code);
        return Ok(response.Token);
    }
    
    
    
    [HttpPost]
    public async Task<IActionResult> Login(IFormCollection form) 
    {
        var response = await UserService.Login(form);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return Ok(response);
        }
        HttpContextAccessor.HttpContext.Session.SetString("JwtToken", response.Token);
        return Ok(response.Token);
    }
}