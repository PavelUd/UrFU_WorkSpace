using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers.Templates;

[Tags("Шаблоны Объектов")]
[Route("api/templates/objects")]
public class ObjectTemplateController
    (IBaseRepository<ObjectTemplate> repository) : TemplateController<ObjectTemplate>(repository);