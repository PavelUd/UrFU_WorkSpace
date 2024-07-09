using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Controllers;

public class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private HttpContext _context;
    private IWorkspaceService Service;
    private IAmenityService AmenityService;
    private IObjectService ObjectService;
    private IVerificationCodeService VerificationCodeService;
    private IWebHostEnvironment _appEnvironment;
    private IReservationService ReservationService;
    private IWorkspaceService WorkspaceService;

    public UserController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment appEnvironment, IWorkspaceService service, IAmenityService amenityService, IObjectService objectService,
        IVerificationCodeService verificationCodeService, IReservationService reservationService, IWorkspaceService workspaceService)
    {
        _context = httpContextAccessor.HttpContext;
        ObjectService = objectService;
        AmenityService = amenityService;
        VerificationCodeService = verificationCodeService;
        _logger = logger;
        _appEnvironment = appEnvironment;
        Service = service;
        WorkspaceService = workspaceService;
        ReservationService = reservationService;
    }
    [Route("{idUser}/verification-codes")]
    public IActionResult GetCodes(int idUser)
    {
        return Ok(VerificationCodeService.GetCodes(idUser).Result);
    }
    
    [HttpPost]
    [Route("update-code")]
    public IActionResult UpdateCode(IFormCollection form)
    {
        var idWorkspace = int.Parse(form["idWorkspace"]);
        var idCode = int.Parse(form["idCode"]);
        return Ok(VerificationCodeService.UpdateCode(idWorkspace, idCode).Result);
    }
    
    [Route("constructor-templates")]
    public IActionResult GetWorkspace()
    {
        var amenityTemplates = AmenityService.GetAmenityTemplates().Result;
        var objectTemplates = ObjectService.GetObjectTemplates().Result;
        ViewBag.Amenities = amenityTemplates;
        ViewBag.Objects =objectTemplates;
        var templates = new Dictionary<string, object>()
        {
            { "amenities", amenityTemplates },
            { "objects", objectTemplates }
        };
        return Ok(JsonHelper.Serialize(templates)); 
    }
    
    [Route("{idUser}/reservations")]
    public IActionResult GetReservation(int idUser)
    {
        return Ok(ReservationService.GetUserReservations(idUser).Result.Where(x => x.IsConfirmed == false));
    }
    
    [HttpPost]
    [Route("{idUser}/confirm-reservation")]
    public IActionResult ConfirmReservation(IFormCollection form)
    {
        var code = form["code"].ToString();
        var id = int.Parse(form["id"]);
        var idWorkspace = int.Parse(form["idWorkspace"]);
        var isConfirmed = ReservationService.VerifyReservation(code, id, idWorkspace);
        return Ok(isConfirmed);
    }
    
    
    [HttpPost]
    [Route("/workspace-create")]
    public IActionResult CreateWorkspace(IFormCollection form, IFormFileCollection uploads)
    {
        var idUser =JwtTokenDecoder.GetUserId(_context.Session.GetString("JwtToken"));
        
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
        var idWorkspace = Service.CreateWorkspace(idUser, baseInfo, operationModeJson,idTemplates, form["objects"],uploads, _appEnvironment);
        if (idWorkspace != 0)
        {
            VerificationCodeService.AddCode(idWorkspace);
        }
        return Redirect("/");
    }
    
    [Route("/users/{idUser}")]
    public IActionResult Profile(int idUser)
    {
        var token = HttpContext.Session.GetString("JwtToken");
        var user = JwtTokenDecoder.Decode(token);
        return View("Profile", user); 
    }
    
    [Route("/users/{idUser}/workspaces")]
    public async Task<IActionResult> UserWorkspaces(int idUser)
    {
        var workspaces = await WorkspaceService.GetAllWorkspaces();
        return View("UserWorkspaces", workspaces.Where(x => x.IdCreator == idUser)); 
    }
    
    [Route("/users/{idUser}/reservations")]
    public async Task<IActionResult> UserReservations(int idUser)
    {
        var views = new List<ReservationView>();
        var reservations =await ReservationService.GetUserReservations(idUser);
        var workspaces = await WorkspaceService.GetAllWorkspaces();
        foreach (var reservation in reservations)
        {
            var workspace = workspaces.FirstOrDefault(x => x.Id == reservation.IdWorkspace);
            var view = new ReservationView()
            {
                Reservation = reservation,
                Name = workspace.Name,
                Image = workspace.Images.FirstOrDefault(),
                WorkspaceObject = workspace.Objects.FirstOrDefault(x => x.Id == reservation.IdObject)

            };
            views.Add(view);
        }
        return View("UserReservations", views); 
    }
}