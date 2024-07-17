using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services;

namespace UrFU_WorkSpace_API.Controllers;
[Tags("Коворкинги")]
[Route("api/workspaces")]
[ApiController]

public class WorkspaceController : Controller
{
    private readonly IWorkspaceRepository WorkspaceRepository;
    private readonly WorkspaceService _workspaceService;
    private ILogger<WorkspaceController> Logger;
    public IMapper mapper { get; set; }
    
    private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
    {
        Converters = { new TimeOnlyJsonConverter() },
    };

    public WorkspaceController(IWorkspaceRepository workspaceRepository, WorkspaceService workspaceService,ILogger<WorkspaceController> logger,  IMapper mapper)
    {
        WorkspaceRepository = workspaceRepository;
        _workspaceService = workspaceService;
        this.mapper = mapper;
        Logger = logger;
    }
    
    /// <summary>
    /// Получить список Коворкингов.
    /// </summary>
    /// <remarks>
    ///  <code>Важно!</code> привет
    /// </remarks>
    /// <param name="idUser">
    ///     ID создателя коворкинга. Поле не обязательное
    /// </param>
    [HttpGet]
    [ProducesResponseType(404, Type = typeof(Error))]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Workspace>))]
    public IActionResult GetWorkspaces(int idUser)
    {
        var result = _workspaceService.GetWorkspaces(idUser);
        if (!result.IsSuccess)
        {
            var error = result.Error;
            return StatusCode((int)error.Status, error);
        }
        Logger.LogInformation(Json(result).ToString());
        return Ok(result.Value);
    }
    
    /// <summary>
    /// Получить Коворкинг по id.
    /// </summary>
    
    [HttpGet("{idWorkspace}")]
    [ProducesResponseType(200, Type = typeof(Workspace))]
    [ProducesResponseType(404, Type = typeof(Error))]
    [ProducesResponseType(500, Type = typeof(Error))]
    public IActionResult GetWorkspaceById(int idWorkspace)
    {
        var result = _workspaceService.GetWorkspaceById(idWorkspace);
        if (!result.IsSuccess)
        {
            var error = result.Error;
            return StatusCode((int)error.Status, error);
        }

        return Ok(result.Value);
    }
    /// <summary>
    /// Частичное обновление коворкинга.
    /// </summary>
    /// <remarks>
    /// Метод предназначен для частичного обновления модели коворкинга
    ///</remarks>
    [HttpPatch("{idWorkspace}")]
    public IActionResult PatchWorkspace([FromBody] JsonPatchDocument<BaseInfo> workspaceComponent, [FromRoute]int idWorkspace)
    {
        var result = _workspaceService.UpdateBaseInfo(idWorkspace, workspaceComponent);
        if (!result.IsSuccess)
        {
            var error = result.Error;
            return StatusCode((int)error.Status, error);
        }

        return NoContent();
    }
    /// <summary>
    /// Создать Коворкинг.
    /// </summary>
    /// <remarks>
    /// Метод предназначен для создания модели коворкинга.&#xA;
    /// <code>Важно!</code>При создании должны соблюдаться следующие условия:
    ///  - Размеры объектов должны быть не менее 1.
    ///  - Координаты расположения объектов ковворкинга не могут быть отрицательными.
    ///  - В режиме работы должно быть не более 7(максимальное количество дней в неделе) объектов, соответствующих дням недели.
    ///  - Номер дня недели не может быть больше 7(максимальное количество дней в неделе).
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(int))]
    public IActionResult CreateWorkspace([FromForm] ModifyWorkspaceDto workspace)
    {
        var form = Request.Form;
        workspace.Objects = form["Objects"].Select(JsonConvert.DeserializeObject<WorkspaceObject>);
        workspace.Amenities = form["Amenities"].Select(JsonConvert.DeserializeObject<WorkspaceAmenity>);
        workspace.OperationMode = form["OperationMode"].Select(x => JsonConvert.DeserializeObject<WorkspaceWeekday>(x,  JsonSettings));
        var result = _workspaceService.CreateWorkspace(workspace);
       
        if (!result.IsSuccess)
        {
            var error = result.Error;
            return StatusCode((int)error.Status, error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Удалить Коворкинг.
    /// </summary>
    
    /// <remarks>
    ///  <code>Важно!</code> При удалении коворкинга также будут удалены все связанные с ним бронирования 
    /// </remarks>
    
    /// <param name="idWorkspace">
    ///     Id Коворкинга
    /// </param>
    
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = typeof(Error))]
    [ProducesResponseType(500, Type = typeof(Error))]
    [HttpDelete("{idWorkspace}")]
    public IActionResult DeleteWorkspace(int idWorkspace)
    {
       var result = _workspaceService.DeleteWorkspace(idWorkspace);
       
       if (!result.IsSuccess)
       {
           var error = result.Error;
           return StatusCode((int)error.Status, error);
       }

       return Ok(result.Value);
    }

    
    /// <summary>
    /// Обновить Коворкинг.
    /// </summary>
    
    
    [ProducesResponseType(204)]
    [ProducesResponseType(400, Type = typeof(Error))]
    [HttpPut("{idWorkspace}")]
    public IActionResult UpdateWorkspace([FromForm] ModifyWorkspaceDto workspace, int idWorkspace)
    {
        var form = Request.Form;
        workspace.Objects = form["Objects"].Select(JsonConvert.DeserializeObject<WorkspaceObject>);
        workspace.Amenities = form["Amenities"].Select(JsonConvert.DeserializeObject<WorkspaceAmenity>);
        workspace.OperationMode = form["OperationMode"].Select(x => JsonConvert.DeserializeObject<WorkspaceWeekday>(x,  JsonSettings));
        var t = _workspaceService.PutWorkspace(workspace, idWorkspace);
        if (!t.IsSuccess)
        {
            return StatusCode((int)t.Error.Status, t.Error);
        }
        return NoContent();
    }
}