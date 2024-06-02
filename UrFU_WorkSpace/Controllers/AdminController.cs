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
    public IActionResult SaveWorkspaceImages(int idUser,IFormCollection form, IFormFileCollection uploads)
    {
        var isSaved = Service.CreateWorkspace(idUser, form, uploads, _appEnvironment);
        return isSaved ? Ok() : BadRequest();
    }
}