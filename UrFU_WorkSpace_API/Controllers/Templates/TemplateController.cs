using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UrFU_WorkSpace_API.Interfaces;

namespace UrFU_WorkSpace_API.Controllers;

public class TemplateController<T> : Controller where T : class
{
    public TemplateController(IBaseRepository<T> repository)
    {
        Repository = repository;
    }

    private IBaseRepository<T> Repository { get; }

    [HttpGet]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetAllTemplates()
    {
        var templates = Repository.FindAll();

        if (templates.IsNullOrEmpty()) return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(templates);
    }
}