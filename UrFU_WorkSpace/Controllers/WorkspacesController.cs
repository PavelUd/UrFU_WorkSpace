using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Controllers;

public class WorkspacesController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public WorkspacesController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [Route("workspaces/{idWorkspace}")]
    public IActionResult GetWorkspace(int idWorkspace)
    {
        var workspace = Workspace.GetWorkSpace(idWorkspace).Result;
        return View("Workspace", workspace); 
    }
    [HttpPost]
    [Route("workspaces/{idWorkspace}/get-time-slots")]
    public async Task<IActionResult> GetTimeSlots([FromRoute] int idWorkspace, IFormCollection form)
    {
        var d = form["date"];
        var t ='\"' + d + '\"';
        var date = JsonConvert.DeserializeObject<DateTime>(t , new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd" });
        var timeSlots = Workspace.GetWorkspaceTimeSlots(idWorkspace, date,  Enum.Parse<TimeType>(form["timeType"].ToString()), form["objectType"]);
        var str = JsonConvert.SerializeObject(timeSlots);
        return Ok(str);
    }

    [HttpPost]
    [Route("workspaces/{idWorkspace}/workspace-objects")]
    public async Task<IActionResult> GetWorkspaceObjects([FromRoute] int idWorkspace, IFormCollection form)
    {
        var dateStr = '\"' + form["date"] + '\"';
        var timeEndStr = '\"' + form["timeStart"] + '\"';
        var timeStartStr = '\"' + form["timeEnd"] + '\"';
        var date = JsonConvert.DeserializeObject<DateTime>(dateStr,
            new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd" });
        var timeEnd = JsonConvert.DeserializeObject<TimeOnly>(timeEndStr);
        var timeStart = JsonConvert.DeserializeObject<TimeOnly>(timeStartStr);
        var objects = Workspace.GetReservationObjects(timeStart, timeEnd, idWorkspace, date);
        return Ok(JsonConvert.SerializeObject(objects));
    }

    [HttpPost]
    [Route("workspaces/{idWorkspace}/reserve")]
    public async Task<IActionResult> Reserve([FromRoute] int idWorkspace, IFormCollection form)
    {
        var idSelectedObject = await Workspace.Reserve(idWorkspace, form);
        if (idSelectedObject == 0)
        {
            return BadRequest();
        }

        return Ok(idSelectedObject);
    }
}