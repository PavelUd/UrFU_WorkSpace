using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Controllers;

public class WorkspacesController : Controller
{
    private Uri baseAdress = new Uri("https://localhost:7077/api/workspaces");
    private readonly HttpClient _client;
    private readonly ILogger<HomeController> _logger;

    public WorkspacesController(ILogger<HomeController> logger)
    {
        _client = new HttpClient();
        _client.BaseAddress = baseAdress;
        _logger = logger;
    }
    [Route("workspaces/{idWorkspace}")]
    public IActionResult GetWorkspace(int idWorkspace)
    {
        var workspace = new Workspace();
        var responseMessage = _client.GetAsync(_client.BaseAddress + $"/workspaces/{idWorkspace}").Result;
        if (responseMessage.IsSuccessStatusCode)
        {
            var data = responseMessage.Content.ReadAsStringAsync().Result;
            workspace = JsonConvert.DeserializeObject<Workspace>(data);
        }

        return View("Workspace", workspace); 
    }
}