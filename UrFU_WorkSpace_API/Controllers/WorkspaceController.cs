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
    private IReservationRepository _reservationRepository;
    
    public WorkspaceController(IWorkspaceRepository workspaceRepository,IReservationRepository reservationRepository,  IMapper mapper)
    {
        this.workspaceRepository = workspaceRepository;
        this.mapper = mapper;
        _reservationRepository = reservationRepository;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<WorkspaceDTO>))]
    public IActionResult GetWorkspaces()
    { 
        var workspaces = mapper.Map<IEnumerable<Workspace>, IEnumerable<WorkspaceDTO>>(workspaceRepository.GetWorkspaces());
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(workspaces);
    }
    
    [HttpPost("create")]
    [ProducesResponseType(201, Type = typeof(IEnumerable<WorkspaceDTO>))]
    public IActionResult AddWorkspaces()
    {
        return Ok();
    }
    
    [HttpGet("{idWorkspace}")]
    [ProducesResponseType(200, Type = typeof(WorkspaceDTO))]
    public IActionResult GetWorkspaceById(int idWorkspace)
    { 
        var workspaces = mapper.Map<Workspace, WorkspaceDTO>(workspaceRepository.GetWorkspaceById(idWorkspace));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(workspaces);
    }
    
    [HttpPatch("{idWorkspace}")]
    [ProducesResponseType(200, Type = typeof(WorkspaceDTO))]
    public IActionResult UpdateWorkspaceById (int idWorkspace)
    { 
        var workspaces = mapper.Map<Workspace, WorkspaceDTO>(workspaceRepository.GetWorkspaceById(idWorkspace));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(workspaces);
    }
    
    [HttpDelete("{idWorkspace}")]
    [ProducesResponseType(204, Type = typeof(WorkspaceDTO))]
    public IActionResult DeleteWorkspaceById (int idWorkspace)
    { 
        var workspaces = mapper.Map<Workspace, WorkspaceDTO>(workspaceRepository.GetWorkspaceById(idWorkspace));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return NoContent();
    }
    
    [HttpGet("{workspaceId}/reservations")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reservation>))]
    public IActionResult GetUserReservations(int workspaceId)
    { 
        var reservations = _reservationRepository.GetUserReservations(workspaceId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reservations);
    }
    
    [HttpPost("{workspaceId}/reserve")]
    [ProducesResponseType(201, Type = typeof(IEnumerable<Reservation>))]
    public IActionResult Reserve(int workspaceId)
    { 
        return Ok();
    }
}