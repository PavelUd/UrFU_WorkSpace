using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers;

[Route("api/verification-codes")]
public class VerificationCodeController : Controller
{
    public IMapper Mapper { get; set; }
    private IVerificationCodeRepository Repository;
    
    public VerificationCodeController(IVerificationCodeRepository repository, IMapper mapper)
    {
        Mapper = mapper;
        Repository = repository;
    }
    
    [HttpPost("create")]
    [ProducesResponseType(201, Type = typeof(VerificationCode))]
    public IActionResult AddCode([FromBody] VerificationCode code)
    {
       Repository.Create(code);
       Repository.Save();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(code);
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<VerificationCodeDto>))]
    public IActionResult GetCode(int idUser)
    {
        var codes = Repository.FindAll()
            .Select(x => Mapper.Map<VerificationCode, VerificationCodeDto>(x));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(codes);
    }
    
    [HttpPut("update")]
    [ProducesResponseType(200, Type = typeof(VerificationCode))]
    public IActionResult UpdateCode([FromBody] VerificationCode code)
    {
        Repository.Update(code);
        Repository.Save();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(code);
    }
}