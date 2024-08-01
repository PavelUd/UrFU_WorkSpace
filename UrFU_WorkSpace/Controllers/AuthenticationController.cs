using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace.Services;

namespace UrFU_WorkSpace.Controllers;



public class AuthenticationController : Controller
{
    private readonly ILogger<AuthenticationController> Logger;
    private AuthenticationService Service;
    
   public AuthenticationController(ILogger<AuthenticationController> logger,AuthenticationService service, IConfiguration configuration)
   {
       Logger = logger;
       Service = service;
   }

    [HttpPost]
    public async Task<IActionResult> Register(IFormCollection form)
    {
        var result = await Service.Register(form);
        if (!result.IsSuccess)
        {
            var error = result.Error;
            return StatusCode(error.Code, error);
        }

        return Accepted();
    }
    
    
    
    [HttpPost]
    public async Task<IActionResult> GetToken(IFormCollection form) 
    {
        var result = await Service.GetToken(form);
        if (!result.IsSuccess)
        {
            var error = result.Error;
            return StatusCode(error.Code, error);
        }
        HttpContext.Session.SetString("JwtToken", result.Value.AccessToken);
        return Ok(result.Value);
    }
    
    [HttpPost]
    public void LogOut()
    {
        HttpContext.Session.Remove("JwtToken");
    } 
}