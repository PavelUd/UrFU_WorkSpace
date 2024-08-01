
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Services;
using UrFU_WorkSpace.Services.Interfaces;
using TimeType = UrFU_WorkSpace.enums.TimeType;

namespace UrFU_WorkSpace.Controllers;

public class WorkspacesController : Controller
{
    private ReviewService _reviewService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IWorkspaceService WorkspaceService;
    private readonly IReservationService ReservationService;
    public WorkspacesController(ILogger<HomeController> logger, ReviewService reviewService, IWorkspaceService workspaceService, IReservationService reservationService, IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
        WorkspaceService = workspaceService;
        ReservationService = reservationService;
        _reviewService = reviewService;
    }

    [Route("workspaces/{idWorkspace}")]
    public async Task<IActionResult> GetWorkspace(int idWorkspace)
    {
        var workspace =await  WorkspaceService.GetWorkspace(idWorkspace);
        var reviews = await _reviewService.GetReviews(idWorkspace);

        var result = workspace.Then(w => reviews.Then(r =>
        {
            w.Reviews = r;
            return w;
        }));

        if (!result.IsSuccess)
        {
            var error = result.Error;
            return StatusCode(error.Code, error);
        }
        
        return View("Workspace", workspace.Value); 
    }
    [HttpPost]
    [Route("workspaces/{idWorkspace}/get-time-slots")]
    public async Task<IActionResult> GetTimeSlots([FromRoute] int idWorkspace, IFormCollection form)
    {
        var date = JsonConvert.DeserializeObject<DateTime>('\"' + form["date"]+ '\"' , new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd" });
        var result = await WorkspaceService.GetWorkspaceTimeSlots(idWorkspace, date,  (TimeType)(int.Parse(form["timeType"])), int.Parse(form["objectType"]));
        if (!result.IsSuccess)
        {
            var error = result.Error;
            return StatusCode(error.Code, error);
        }
        return Ok(JsonConvert.SerializeObject(result.Value));
    }

    [HttpPost]
    [Route("workspaces/{idWorkspace}/add-review")]

    public async Task<IActionResult> AddReview([FromRoute] int idWorkspace, IFormCollection form)
    {
        var idUser = JwtTokenDecoder.GetUserId(httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
        var stars = form["starsCount"].IsNullOrEmpty() ? "0" : form["starsCount"].ToString();
        var text = form["text"];
        var date = JsonHelper.Deserialize<DateOnly>('\"' + form["date"] + '\"');
        
       await _reviewService.AddReview(new Models.Review()
        {
            IdUser = idUser,
            IdWorkspace = idWorkspace,
            Message = text,
            Estimation = int.Parse(stars),
            Date = date
            
        }, httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
       
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Redirect("/workspaces/" + idWorkspace);
    }
    
    
    [HttpPost]
    [Route("workspaces/{idWorkspace}/workspace-objects")]
    public async Task<IActionResult> GetWorkspaceObjects([FromRoute] int idWorkspace, IFormCollection form)
    {
        var dateStr = '\"' + form["date"] + '\"';
        var timeEndStr = '\"' + form["timeStart"] + '\"';
        var timeStartStr = '\"' + form["timeEnd"] + '\"';
        var idTemplate = int.Parse(form["objectType"]);
        var date = JsonConvert.DeserializeObject<DateTime>(dateStr,new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd" });
        var timeEnd = JsonConvert.DeserializeObject<TimeOnly>(timeEndStr);
        var timeStart = JsonConvert.DeserializeObject<TimeOnly>(timeStartStr);
        
        var result = await WorkspaceService.GetWorkspaceObjects(idWorkspace,idTemplate, new DateOnly(date.Year, date.Month, date.Day), timeStart, timeEnd);
        if (!result.IsSuccess)
        {
            var error = result.Error;
            return StatusCode(error.Code, error);
        }
        return Ok(JsonConvert.SerializeObject(result.Value, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver() 
        }));
    }

    [HttpPost]
    [Route("workspaces/{idWorkspace}/reserve")]
    public async Task<IActionResult> Reserve([FromRoute] int idWorkspace, IFormCollection form)
    {
        var token = httpContextAccessor.HttpContext.Session.GetString("JwtToken");
        var creationResult = await ReservationService.Reserve(idWorkspace, form, token);
        var result = creationResult.Then(id => ReservationService.GetReservation(id, token));
        
        if (!result.IsSuccess)
        {
            var error = result.Error;
            return StatusCode(error.Code, error);
        }
        return Ok(JsonConvert.SerializeObject(result.Value, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver() 
        }));
    }
}