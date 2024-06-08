using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Controllers;

[Authorize(AuthorizationStatus.Admin)]
public class AdminController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private HttpContext _context;
    private IWorkspaceService Service;
    private IWebHostEnvironment _appEnvironment;

    public AdminController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment appEnvironment, IWorkspaceService service)
    {
        _context = httpContextAccessor.HttpContext;
        _logger = logger;
        _appEnvironment = appEnvironment;
        Service = service;
    }
    
    [Route("{idUser}/constructor")]
    public IActionResult GetWorkspace(int idUser)
    {
        return View("WorkspaceConstructor"); 
    }
    
    [HttpPost]
    [Route("{idUser}/workspace-create")]
    public IActionResult CreateWorkspace(int idUser,IFormCollection form, IFormFileCollection uploads)
    {
        var baseInfo = new Dictionary<string, object>()
        {
            {"name", form["name"].ToString() },
            {"description", form["description"].ToString()},
            {"institute", form["institute"].ToString()},
            {"address", form["address"].ToString()},
        };
       
        var operationModeJson = new List<(string, string)>()
        {
            (form["mondayStart"], form["mondayEnd"]),
            (form["tuesdayStart"], form["tuesdayEnd"]),
            (form["wednesdayStart"], form["wednesdayEnd"]),
            (form["thursdayStart"], form["thursdayEnd"]),
            (form["fridayStart"], form["fridayEnd"]),
            (form["saturdayStart"], form["saturdayEnd"]),
            (form["sundayStart"], form["sundayEnd"]),
        };
        
        var isSaved = Service.CreateWorkspace(idUser, baseInfo, operationModeJson, form["objects"],uploads, _appEnvironment);
        return isSaved ? Ok() : BadRequest();
    }
}