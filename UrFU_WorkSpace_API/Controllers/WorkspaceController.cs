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
        return Ok(objects);
    }
}