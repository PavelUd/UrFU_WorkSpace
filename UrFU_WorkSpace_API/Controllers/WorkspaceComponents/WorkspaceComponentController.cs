using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers;



public class WorkspaceComponentController<T> : Controller where T : class, IWorkspaceComponent
{
    private IBaseRepository<T> Repository { get; }

    public WorkspaceComponentController(IBaseRepository<T> repository)
    {
        Repository = repository;
    }
    
    [HttpGet]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetComponents([FromRoute]int idWorkspace)
    { 
        var operationMode = Repository.FindByCondition(x => x.IdWorkspace == idWorkspace);
        
        if (operationMode.IsNullOrEmpty())
        {
            return NotFound();
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(operationMode);
    }
    
    [HttpGet("{idComponent}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetComponentById([FromRoute] int idComponent, [FromRoute] int idWorkspace)
    {
        var weekday = Repository.FindByCondition(x => x.Id == idComponent && x.IdWorkspace == idWorkspace);

        if (weekday.IsNullOrEmpty())
        {
            return NotFound();
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(weekday.First());
    }
    
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(400)]
    public IActionResult CreateComponents([FromBody] IEnumerable<T> operationMode)
    {
        foreach (var weekday in operationMode)
        {
            Repository.Create(weekday);
        }

        var isSaved = Repository.Save();
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(isSaved);
    }
    
    [HttpPut]
    [Route("update")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(400)]
    public IActionResult UpdateComponents([FromBody] IEnumerable<T> operationMode)
    {
        foreach (var weekday in operationMode)
        {
            Repository.Update(weekday);
        }

        var isSaved = Repository.Save();
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(isSaved);
    }
    
    [HttpDelete]
    [Route("delete")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(400)]
    public IActionResult DeleteComponents([FromBody] IEnumerable<T> operationMode)
    {
        foreach (var weekday in operationMode)
        {
            Repository.Delete(weekday);
        }

        var isSaved = Repository.Save();
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(isSaved);
    }
}