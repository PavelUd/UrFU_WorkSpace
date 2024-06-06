using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers;
[Route("api/workspaces")]
[ApiController]

public class WorkspaceController : Controller
{
    private readonly IWorkspaceRepository WorkspaceRepository;
    public IMapper mapper { get; set; }

    public WorkspaceController(IWorkspaceRepository workspaceRepository,  IMapper mapper)
    {
        WorkspaceRepository = workspaceRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Workspace>))]
    public IActionResult GetWorkspaces()
    { 
        var workspaces = WorkspaceRepository.FindAll();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(workspaces);
    }
    
    [HttpGet("{idWorkspace}")]
    [ProducesResponseType(200, Type = typeof(Workspace))]
    public IActionResult GetWorkspaceById(int idWorkspace)
    {
        var workspace = WorkspaceRepository.FindByCondition(x => x.Id == idWorkspace).FirstOrDefault();
        if (workspace == null)
        {
            return NotFound(ModelState);
        }
        var workspaces = workspace;
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(workspaces);
    }

    [HttpGet("{idWorkspace}/objects")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Image>))]
    public IActionResult GetWorkspaceObjects(int idWorkspace)
    {
        var objects = WorkspaceRepository.GetWorkspaceObjects(idWorkspace);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(objects);
    }
    
    [HttpGet("{idWorkspace}/amenities")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<WorkspaceAmenity>))]
    public IActionResult GetWorkspaceAmenities(int idWorkspace)
    {
        var amenities = WorkspaceRepository.GetWorkspaceAmenities(idWorkspace);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(amenities);
    }

    [HttpPut("add-weekday")]
    public IActionResult AddWeekday([FromBody] WorkspaceWeekday weekday)
    {
        var isSaved = WorkspaceRepository.AddWeekday(weekday);
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(isSaved);
    }
    
    [HttpPut("add-object")]
    public IActionResult AddObject([FromBody] WorkspaceObject obj)
    {
        var isSaved = WorkspaceRepository.AddObject(obj);
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(isSaved);
    }
    
    [HttpPut("add-image")]
    public IActionResult AddImage([FromBody] Image image)
    {
        var isSaved = WorkspaceRepository.AddImage(image);
        
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(isSaved);
    }
    
    [HttpPut("add-workspace")]
    public IActionResult AddWorkspace([FromBody] Workspace workspace)
    {
        WorkspaceRepository.Create(workspace);
        WorkspaceRepository.Save();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(workspace.Id);
    }
    
    [HttpGet("{idWorkspace}/operation-mode")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<WorkspaceWeekday>))]
    public IActionResult GetWorkspaceOperationMode(int idWorkspace)
    {
        var amenities = WorkspaceRepository.GetWorkspaceOperationMode(idWorkspace);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(amenities);
    }
}