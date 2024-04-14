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
    
    public WorkspaceController(IWorkspaceRepository workspaceRepository, IMapper mapper)
    {
        this.workspaceRepository = workspaceRepository;
        this.mapper = mapper;
    }

    public IMapper mapper { get; set; }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<WorkspaceDTO>))]
    public IActionResult GetWorkspaces()
    { 
        var workspaces = mapper.Map<IEnumerable<Workspace>, IEnumerable<WorkspaceDTO>>(workspaceRepository.GetWorkspaces());
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(workspaces);
    }
}