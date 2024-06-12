using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers;



public class TemplateController<T> : Controller where T : class
{
    private IBaseRepository<T> Repository { get; }

    public TemplateController(IBaseRepository<T> repository)
    {
        Repository = repository;
    }
    
    [HttpGet]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetAllTemplates()
    { 
        var templates = Repository.FindAll();
        
        if (templates.IsNullOrEmpty())
        {
            return NotFound();
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(templates);
    }
    
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(400)]
    public IActionResult CreateComponents(T template)
    {
        Repository.Create(template);
        var isSaved = Repository.Save();
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(isSaved);
    }
    
    [HttpPut]
    [Route("update")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(400)]
    public IActionResult UpdateComponents(T template)
    {
        Repository.Update(template);
        var isSaved = Repository.Save();
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(isSaved);
    }
    
    [HttpDelete]
    [Route("delete")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(400)]
    public IActionResult DeleteComponents(T template)
    {
        Repository.Delete(template);

        var isSaved = Repository.Save();
        if (!ModelState.IsValid || !isSaved)
            return BadRequest(ModelState);

        return Ok(isSaved);
    }
}
