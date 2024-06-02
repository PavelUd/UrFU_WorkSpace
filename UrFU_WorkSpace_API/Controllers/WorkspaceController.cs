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
    private readonly IWorkspaceRepository workspaceRepository;
    public IMapper mapper { get; set; }

    public WorkspaceController(IWorkspaceRepository workspaceRepository,  IMapper mapper)
    {
        this.workspaceRepository = workspaceRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<WorkspaceDTO>))]
    public IActionResult GetWorkspaces()
    { 
        var workspaces = mapper.Map<IEnumerable<Workspace>, IEnumerable<WorkspaceDTO>>(workspaceRepository.FindAll());
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(workspaces);
    }
    
    [HttpPost("save")]
    public IActionResult SaveWorkspaces()
    {
        var isSaved = workspaceRepository.Save();

        return isSaved ? Ok() : BadRequest();
    }
    
    [HttpGet("{idWorkspace}")]
    [ProducesResponseType(200, Type = typeof(WorkspaceDTO))]
    public IActionResult GetWorkspaceById(int idWorkspace)
    {
        var workspace = workspaceRepository.FindByCondition(x => x.Id == idWorkspace).FirstOrDefault();
        if (workspace == null)
        {
            return NotFound(ModelState);
        }
        var workspaces = mapper.Map<Workspace, WorkspaceDTO>(workspace);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(workspaces);
    }

    [HttpGet("{idWorkspace}/objects")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<WorkspaceObject>))]
    public IActionResult GetWorkspaceObjects(int idWorkspace)
    {
        var objects = workspaceRepository.GetWorkspaceObjects(idWorkspace);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(objects);
    }
    
    [HttpGet("{idWorkspace}/amenities")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<WorkspaceAmenity>))]
    public IActionResult GetWorkspaceAmenities(int idWorkspace)
    {
        var amenities = workspaceRepository.GetWorkspaceAmenities(idWorkspace);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(amenities);
    }

    [HttpPut("add-weekday")]
    public IActionResult AddWeekday([FromBody] WorkspaceWeekday weekday)
    {
        var isSaved = workspaceRepository.AddWeekday(weekday);
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(isSaved);
    }
    
    [HttpPut("add-object")]
    public IActionResult AddObject([FromBody] WorkspaceObject obj)
    {
        var isSaved = workspaceRepository.AddObject(obj);
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(isSaved);
    }
    
    [HttpPut("add-image")]
    public IActionResult AddObject([FromBody] WorkspaceImage image)
    {
        var isSaved = workspaceRepository.AddImage(image);
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(isSaved);
    }
    
    [HttpPut("add-workspace")]
    public IActionResult AddWorkspace([FromBody] Workspace workspace)
    {
        workspaceRepository.Create(workspace);
        var isSaved = workspaceRepository.Save();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(workspace.Id);
    }
    
    [HttpGet("{idWorkspace}/operation-mode")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<WorkspaceWeekday>))]
    public IActionResult GetWorkspaceOperationMode(int idWorkspace)
    {
        var amenities = workspaceRepository.GetWorkspaceOperationMode(idWorkspace);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(amenities);
    }
}