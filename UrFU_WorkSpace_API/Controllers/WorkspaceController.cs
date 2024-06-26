using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    [HttpPatch("{idWorkspace}/update-rating")]
    public IActionResult UpdateWorkspaceRating([FromQuery]double rating, [FromRoute]int idWorkspace)
    {
       var i = WorkspaceRepository.FindByCondition(x => x.Id == idWorkspace)
            .Where(x => x.Id == idWorkspace)
            .ExecuteUpdate(b => b.SetProperty(x => x.Rating, rating));

        return Ok(i);
    }

    [HttpPost("create")]
    [ProducesResponseType(204, Type = typeof(Workspace))]
    public IActionResult CreateWorkspace([FromBody] Workspace workspace)
    {
        WorkspaceRepository.Create(workspace);
        var isSaved = WorkspaceRepository.Save();
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(workspace.Id);
    }
}