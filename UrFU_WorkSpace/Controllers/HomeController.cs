using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace.Models;
using User = UrFU_WorkSpace.Models.User;
using Workspace = UrFU_WorkSpace.Models.Workspace;

namespace UrFU_WorkSpace.Controllers;
public class HomeController : Controller
{
    private Uri baseAdress = new Uri("https://localhost:7077/api");
    private readonly HttpClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _client = new HttpClient();
        _client.BaseAddress = baseAdress;
        _logger = logger;
    }
    [HttpPost]
    public IActionResult Register(IFormCollection form)
    {
        var user = new User(_httpContextAccessor.HttpContext);
        var t = user.Register(form);
        return View(t.ToString());
    }
    [HttpPost]
    public async Task<IActionResult> Login(IFormCollection form)
    {
        var user = new User(_httpContextAccessor.HttpContext);
        await user.Login(form);

        if (_httpContextAccessor.HttpContext.Session.GetString("JwtToken") != null)
        {
            return Ok(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
        }
        else
        {
            return BadRequest("Login failed");
        }
    }

    public IActionResult Index()
    {
        var workspaces = new List<Workspace>();
        var responseMessage = _client.GetAsync(_client.BaseAddress + "/workspaces").Result;
        if (responseMessage.IsSuccessStatusCode)
        {
            var data = responseMessage.Content.ReadAsStringAsync().Result;
            workspaces = JsonConvert.DeserializeObject<List<Workspace>>(data);
        }
        return View(workspaces);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}