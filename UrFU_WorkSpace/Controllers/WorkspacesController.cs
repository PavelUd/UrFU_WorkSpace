using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Controllers;

public class WorkspacesController : Controller
{
    private Uri baseAdress = new Uri("https://localhost:7077/api/workspaces");
    private readonly ILogger<HomeController> _logger;

    public WorkspacesController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [Route("workspaces/{idWorkspace}")]
    public IActionResult GetWorkspace(int idWorkspace)
    {
        var workspace = new Workspace();
        var responseMessage = HttpRequestSender.SentGetRequest(baseAdress + $"/{idWorkspace}").Result;
        if (responseMessage.IsSuccessStatusCode)
        {
            var data = responseMessage.Content.ReadAsStringAsync().Result;
            workspace = JsonConvert.DeserializeObject<Workspace>(data);
        }

        return View("Workspace", workspace); 
    }
}