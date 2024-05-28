using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Controllers;

[Authorize(AuthorizationStatus.Admin)]
public class AdminController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private HttpContext _context;
    private IWebHostEnvironment _appEnvironment;

    public AdminController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment appEnvironment, IConfiguration configuration)
    {
        Workspace.baseAdress = new Uri(configuration["apiAddress"]);
        _context = httpContextAccessor.HttpContext;
        _logger = logger;
        _appEnvironment = appEnvironment;
    }
    
    [Route("{idUser}/constructor")]
    public IActionResult GetWorkspace(int idUser)
    {
        return View("WorkspaceConstructor"); 
    }
    
    [HttpPost]
    [Route("{idUser}/workspace-create")]
    public IActionResult SaveWorkspaceImages(int idUser,IFormCollection form, IFormFileCollection uploads)
    {
        var urls = Workspace.SaveImages(_appEnvironment, uploads);
        
        var idCreatedWorkspace = Workspace.CreateWorkspace(form, idUser);
        if (idCreatedWorkspace == 0)
        {
            return BadRequest();
        }
        var objIsSaved = Workspace.CreateObjects(idCreatedWorkspace ,form["objects"]);
        var weekDaysIsSaved = Workspace.CreateOperationMode(form ,idCreatedWorkspace);
        var imagesIsSaved = Workspace.AddWorkspaceImages(idCreatedWorkspace ,urls);

        if (!objIsSaved || !weekDaysIsSaved || !imagesIsSaved)
        {
            return BadRequest();
        }
        return Ok();
    }
}