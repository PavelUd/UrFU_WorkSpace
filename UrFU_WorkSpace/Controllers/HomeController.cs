using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace.Models;
using Workspace = UrFU_WorkSpace.Models.Workspace;

namespace UrFU_WorkSpace.Controllers;

public class HomeController : Controller
{
    private Uri baseAdress = new Uri("https://localhost:7077/api");
    private readonly HttpClient _client;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _client = new HttpClient();
        _client.BaseAddress = baseAdress;
        _logger = logger;
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