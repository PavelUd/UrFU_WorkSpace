using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
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

    private readonly WorkspaceService _workspaceService;
    private ILogger<WorkspaceController> Logger;
    public IMapper mapper { get; set; }
    
    private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
    {
        Converters = { new TimeOnlyJsonConverter() },
    };

    public WorkspaceController(WorkspaceService workspaceService,
        ILogger<WorkspaceController> logger,  
        IMapper mapper)
    {
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
    /// <param name="isFull">Если значение <c>true</c>, то предоставляется полная информация о коворкинге. Если <c>false</c>, предоставляется только базовая часть информации.</param>
    [HttpGet]
    [ProducesResponseType(404, Type = typeof(Error))]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Workspace>))]
    public IActionResult GetWorkspaces(int idUser, bool isFull = false)
    {
        var result = _workspaceService.GetWorkspaces(idUser, isFull);
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
    public IActionResult GetWorkspaceById(int idWorkspace, bool isFull = false)
    {
        var result = _workspaceService.GetWorkspaceById(idWorkspace, isFull);
        if (!result.IsSuccess)
        {
            var error = result.Error;
            return StatusCode((int)error.Status, error);
        }

        return Ok(result.Value);
    }
    
     /// <summary>
     /// Получить объекты коворкинга
     /// </summary>
     /// <remarks>
     /// Возвращает коллекцию объектов, принадлежащих Коворкингу
     /// </remarks>
     
     [HttpGet("{idWorkspace}/objects")]
     [ProducesResponseType(200, Type = typeof(IEnumerable<WorkspaceObject>))]
     [ProducesResponseType(404, Type = typeof(Error))]
     [ProducesResponseType(500, Type = typeof(Error))]
     public IActionResult GetWorkspaceObjects(int idWorkspace)
     {
         var result = _workspaceService.GetObjects(idWorkspace);
         if (!result.IsSuccess)
         {
             var error = result.Error;
             return StatusCode((int)error.Status, error);
         }

         return Ok(result.Value);
     }
     
     /// <summary>
     /// Получить  удобства коворкинга
     /// </summary>
     /// <remarks>
     /// Возвращает  коллекцию удобств, предоставляемых в коворкинг
     /// </remarks>

     [HttpGet("{idWorkspace}/amenities")]
     [ProducesResponseType(200, Type = typeof(IEnumerable<WorkspaceAmenity>))]
     [ProducesResponseType(404, Type = typeof(Error))]
     [ProducesResponseType(500, Type = typeof(Error))]
     public IActionResult GetWorkspaceAmenities(int idWorkspace)
     {
         var result = _workspaceService.GetAmenities(idWorkspace);
         if (!result.IsSuccess)
         {
             var error = result.Error;
             return StatusCode((int)error.Status, error);
         }

         return Ok(result.Value);
     }
     
     /// <summary>
     /// Получить режим работы коворкинга
     /// </summary>
     /// <remarks>
     /// Возвращает Режим работы коворкинга по дням недели
     /// </remarks>
     [HttpGet("{idWorkspace}/operation-mode")]
     [ProducesResponseType(200, Type = typeof(IEnumerable<WorkspaceWeekday>))]
     [ProducesResponseType(404, Type = typeof(Error))]
     [ProducesResponseType(500, Type = typeof(Error))]
     public IActionResult GetWorkspaceOperationMode(int idWorkspace)
     {
         var result = _workspaceService.GetOperationMode(idWorkspace);
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
    /// Метод предназначен для частичного обновления модели коворкинга.&#xA;
    /// Поля, короторые можно обновить:
    /// - <term><c>name</c></term>
    ///- <term><c>description</c></term>
    ///- <term><c>rating</c></term>
    ///- <term><c>address</c></term>
    ///- <term><c>idCreator</c></term>
    ///- <term><c>institute</c></term>
    /// </remarks>
    [Authorize(Roles =  nameof(Role.Admin))]
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
    /// Создать коворкинг.
    /// </summary>
    /// <remarks>
    /// Метод предназначен для создания модели коворкинга.
    /// <para>Поля для создания коворкинга:</para>
    ///
    /// - <c>name</c> Название коворкинга.
    /// - <c>description</c> Описание коворкинга, включающее информацию о его услугах и особенностях.
    /// - <c>institute</c> Институт, в кототром расположен коворкинг
    /// - <c>objects</c> Коллекция объектов  коворкинга.
    /// - <c>amenities</c> Коллекция удобств, предоставляемых в коворкинге.
    /// - <c>operationMode</c>  Режим работы коворкинга по дням недели.
    /// - <c>imageFiles</c>  Коллекция изображений, связанных с коворкингом
    /// - <c>address</c>  Физический адрес коворкинга.
    /// - <c>privacy</c>  Уровень конфиденциальности или приватности коворкинга (целочисленное значение).
    /// - <c>idCreator</c>  Идентификатор пользователя, создавшего запись о коворкинге (целочисленное значение, обязательное поле)
    ///&#xA;&#xA;
    /// <para><code>Важно!</code></para> При создании коворкинга необходимо соблюдать следующие условия:
    /// - <description>Размеры объектов должны быть не менее 1.</description>
    /// - <description>Координаты расположения объектов коворкинга не могут быть отрицательными.</description>
    /// - <description>В режиме работы должно быть не более 7 объектов, соответствующих дням недели (максимальное количество дней в неделе).</description>
    /// - <description>Номер дня недели не может быть больше 7 (максимальное количество дней в неделе).</description>
    /// - <description>в объктах не нужно устанавливать поле id и idWorkspace</description>
    /// </remarks>
     
    [HttpPost]
    [Authorize(Roles =  nameof(Role.Admin))]
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
     /// Обновить коворкинг.
     /// </summary>
     /// <remarks>
     /// Этот метод позволяет обновить информацию о коворкинге.
     /// <para>Чтобы выполнить обновление, необходимо указать уникальный идентификатор (<c>id</c>) коворкинга.</para>
     /// <para>Все поля коворкинга будут полностью переписаны новыми значениями, предоставленными в запросе.</para>
     /// </remarks>
    
     [Authorize(Roles =  nameof(Role.Admin))]
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
     
    /// <summary>
    /// Удалить Коворкинг.
    /// </summary>
    
    /// <remarks>
    /// <para>Этот метод полностью удаляет запись о коворкинге из системы. Следует учитывать, что это действие необратимо.</para>&#xA;
    ///  <code>Важно!</code> При удалении коворкинга также будут удалены все связанные с ним бронирования 
    /// </remarks>
    
    /// <param name="idWorkspace">
    ///     Id Коворкинга
    /// </param>
    
    [Authorize(Roles =  nameof(Role.Admin))]
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
}