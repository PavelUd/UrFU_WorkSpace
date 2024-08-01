using System.Diagnostics;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services;
using UrFU_WorkSpace.Services.Interfaces;
using User = UrFU_WorkSpace.Models.User;
using Workspace = UrFU_WorkSpace.Models.Workspace;

namespace UrFU_WorkSpace.Controllers;
public class HomeController
    : Controller
{
    private readonly Uri _baseAddress;
    private readonly ILogger<HomeController> _logger;
    private readonly IWorkspaceService _workspaceService;

    public HomeController(ILogger<HomeController> logger,IWorkspaceService workspaceService)
    {
        _logger = logger;
        _workspaceService = workspaceService;
    }

    public IActionResult Index()
    {
        var result = _workspaceService.GetAllWorkspaces().Result;
        if (!result.IsSuccess)
        {
            var error = result.Error;
            return StatusCode(error.Code, error);
        }
        return View(result.Value);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}