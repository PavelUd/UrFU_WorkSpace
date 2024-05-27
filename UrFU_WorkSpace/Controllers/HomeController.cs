using System.Diagnostics;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using User = UrFU_WorkSpace.Models.User;
using Workspace = UrFU_WorkSpace.Models.Workspace;

namespace UrFU_WorkSpace.Controllers;
public class HomeController
    : Controller
{
    private readonly Uri _baseAddress = new Uri("https://localhost:7077/api");
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;

    }

    public IActionResult Index()
    {
        var workspaces = new List<Workspace>();
        var responseMessage = HttpRequestSender.SendRequest(_baseAddress + "/workspaces", RequestMethod.Get);
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new TimeOnlyJsonConverter());
        if (!responseMessage.Result.IsSuccessStatusCode)
        {
            return View(workspaces);
        }
        var data = responseMessage.Result.Content.ReadAsStringAsync().Result;
        workspaces = JsonConvert.DeserializeObject<List<Workspace>>(data, settings);
        return View(workspaces);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}