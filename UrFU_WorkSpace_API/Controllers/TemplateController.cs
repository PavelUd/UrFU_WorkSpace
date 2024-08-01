using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Controllers;

[Tags("Шаблоны")]
[Route("api/templates")]

public class TemplateController : Controller
{
    private readonly ITemplateService _service;
    public TemplateController(ITemplateService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = nameof(Role.Admin))]
    [ProducesResponseType(400)]
    public IActionResult GetAllTemplates()
    {
        var result = _service.GetAll();

        if (!result.IsSuccess) 
            return StatusCode((int)result.Error.HttpStatusCode, result.Error);

        return Ok(result.Value);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = nameof(Role.Admin))]
    [ProducesResponseType(404)]
    public IActionResult GetTemplate(int id)
    {
        var result = _service.GetTemplateById(id);

        if (!result.IsSuccess) 
            return StatusCode((int)result.Error.HttpStatusCode, result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Admin))]
    [ProducesResponseType(201)]
    public IActionResult CreateTemplate([FromForm] ModifyTemplateDto template)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var result = _service.CreateTemplate(template);
        
        if (!result.IsSuccess) 
            return StatusCode((int)result.Error.HttpStatusCode, result.Error);

        return Created();
    }
    
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpDelete("{id}")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(400)]
    public IActionResult DeleteComponents(int id)
    {
       var result = _service.Delete(id);
       if (!result.IsSuccess)
       {
           var error = result.Error;
           return StatusCode((int)error.HttpStatusCode, error);
       }

       return NoContent();
    }
}