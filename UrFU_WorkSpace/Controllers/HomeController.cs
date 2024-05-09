using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using User = UrFU_WorkSpace.Models.User;
using Workspace = UrFU_WorkSpace.Models.Workspace;

namespace UrFU_WorkSpace.Controllers;
public class HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
    : Controller
{
    private readonly Uri _baseAddress = new Uri("https://localhost:7077/api");
    private readonly ILogger<HomeController> _logger = logger;
    
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

    public IActionResult Index()
    {
        var workspaces = new List<Workspace>();
        var responseMessage = HttpRequestSender.SentGetRequest(_baseAddress + "/workspaces");
        if (!responseMessage.Result.IsSuccessStatusCode)
        {
            return View(workspaces);
        }
        var data = responseMessage.Result.Content.ReadAsStringAsync().Result;
        workspaces = JsonConvert.DeserializeObject<List<Workspace>>(data);
        return View(workspaces);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}