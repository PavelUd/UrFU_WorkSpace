using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Controllers;

public class UserController : Controller
{
    private IReservationService ReservationService;
    private IWorkspaceService WorkspaceService;

    public UserController(IReservationService reservationService, IWorkspaceService workspaceService)
    {
        WorkspaceService = workspaceService;
        ReservationService = reservationService;
    }
    
    [Route("constructor-templates")]
    public IActionResult GetWorkspace()
    {
        var templates = new Dictionary<string, object>()
        {
        };
        return Ok(JsonHelper.Serialize(templates)); 
    }
    
    [Authorize(Roles =nameof(Role.User) + "," + nameof(Role.Admin))]
    [Route("{idUser}/reservations")]
    public IActionResult GetReservation(int idUser)
    {
        return Ok(ReservationService.GetReservations(idUser,  HttpContext.Session.GetString("JwtToken")).Result.Value.Where(x => x.IsConfirmed == false));
    }
    
    [HttpPost]
    [Route("{idUser}/confirm-reservation")]
    public IActionResult ConfirmReservation(IFormCollection form)
    {
        var code = form["code"].ToString();
        var id = int.Parse(form["id"]);
        var idWorkspace = int.Parse(form["idWorkspace"]);
        return Ok(true);
    }
    
    
    [HttpPost]
    [Route("/workspace-create")]
    public IActionResult CreateWorkspace(IFormCollection form, IFormFileCollection uploads)
    {
        var idUser =JwtTokenDecoder.GetUserId(HttpContext.Session.GetString("JwtToken"));
        
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
        var idTemplates = JsonHelper.Deserialize<List<int>>(form["idTemplate"].ToString());
 //       var idWorkspace = Service.CreateWorkspace(idUser, baseInfo, operationModeJson,idTemplates, form["objects"],uploads, _appEnvironment);
//        if (idWorkspace != 0)
        {
 //           VerificationCodeService.AddCode(idWorkspace);
        }
        return Redirect("/");
    }
    
    [Authorize(Roles =nameof(Role.User) + "," + nameof(Role.Admin))]
    [Route("/users/{idUser}")]
    public IActionResult Profile(int idUser)
    {
        var token = HttpContext.Session.GetString("JwtToken");
        var user = JwtTokenDecoder.Decode(token);
        return View("Profile", user); 
    }
    
    [Authorize(Roles = nameof(Role.Admin))]
    [Route("/users/{idUser}/workspaces")]
    public async Task<IActionResult> UserWorkspaces(int idUser)
    {
        var workspaces = await WorkspaceService.GetAllWorkspaces();
        return View("UserWorkspaces", workspaces.Value.Where(x => x.IdCreator == idUser)); 
    }
    
    [Route("/users/{idUser}/reservations")]
    public async Task<IActionResult> UserReservations()
    {
        var views = new List<Reservation>();
        var reservations =await ReservationService.GetReservations( HttpContext.Session.GetString("JwtToken"), true);
        return View("UserReservations", reservations.Value); 
    }
}