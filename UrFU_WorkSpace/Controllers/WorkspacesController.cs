
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services;
using UrFU_WorkSpace.Services.Interfaces;
using Review = UrFU_WorkSpace_API.Models.Review;

namespace UrFU_WorkSpace.Controllers;

public class WorkspacesController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ReviewService _reviewService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IWorkspaceService WorkspaceService;
    private readonly IReservationService ReservationService;
    public WorkspacesController(ILogger<HomeController> logger, ReviewService reviewService, IWorkspaceService workspaceService, IReservationService reservationService, IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
        WorkspaceService = workspaceService;
        ReservationService = reservationService;
        _logger = logger;
        _reviewService = reviewService;
    }
    [Route("workspaces/{idWorkspace}")]
    public IActionResult GetWorkspace(int idWorkspace)
    {
        var workspace = WorkspaceService.GetWorkspace(idWorkspace);
        workspace.Reviews = _reviewService.GetReviews(idWorkspace);
        return View("Workspace", workspace); 
    }
    [HttpPost]
    [Route("workspaces/{idWorkspace}/get-time-slots")]
    public async Task<IActionResult> GetTimeSlots([FromRoute] int idWorkspace, IFormCollection form)
    {
        var d = form["date"];
        var t ='\"' + d + '\"';
        var date = JsonConvert.DeserializeObject<DateTime>(t , new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd" });
        var timeSlots = WorkspaceService.GetWorkspaceTimeSlots(idWorkspace, date,  Enum.Parse<TimeType>(form["timeType"].ToString()), int.Parse(form["objectType"]));
        var str = JsonConvert.SerializeObject(timeSlots);
        return Ok(str);
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
            
        });

       var newRating = _reviewService.RecalculateRating(idWorkspace);
       WorkspaceService.UpdateWorkspaceRating(idWorkspace, newRating);
       
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
        
        var objects = WorkspaceService.GetReservedObjects(timeStart, timeEnd, idWorkspace, date, idTemplate);
        return Ok(JsonConvert.SerializeObject(objects, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver() 
        }));
    }

    [HttpPost]
    [Route("workspaces/{idWorkspace}/reserve")]
    public async Task<IActionResult> Reserve([FromRoute] int idWorkspace, IFormCollection form)
    {
        var reservation = ReservationService.Reserve(idWorkspace, form).Result;
        if (reservation == null)
        {
            return BadRequest();
        }

        return Ok(JsonConvert.SerializeObject(reservation, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver() 
        }));
    }
}