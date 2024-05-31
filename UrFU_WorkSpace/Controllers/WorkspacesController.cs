
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services;
using Review = UrFU_WorkSpace_API.Models.Review;

namespace UrFU_WorkSpace.Controllers;

public class WorkspacesController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ReviewRepository _reviewRepository;
    private readonly IHttpContextAccessor httpContextAccessor;

    public WorkspacesController(ILogger<HomeController> logger, IConfiguration configuration, ReviewRepository reviewRepository, IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
        Workspace.baseAdress = new Uri(configuration["apiAddress"]);
        _logger = logger;
        _reviewRepository = reviewRepository;
    }
    [Route("workspaces/{idWorkspace}")]
    public IActionResult GetWorkspace(int idWorkspace)
    {
        var workspace = Workspace.GetWorkSpace(idWorkspace).Result;
        workspace.Reviews = _reviewRepository.GetByIdWorkspace(idWorkspace);
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
    [Route("workspaces/{idWorkspace}/add-review")]

    public IActionResult AddReview([FromRoute] int idWorkspace, IFormCollection form)
    {
        var idUser = JwtTokenDecoder.GetUserId(httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
        var stars = form["starsCount"];
        var text = form["text"];
        
        _reviewRepository.AddReview(new Models.Review()
        {
            IdUser = int.Parse(idUser),
            IdWorkspace = idWorkspace,
            Message = text,
            Estimation = int.Parse(stars)
        });
        
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