using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers.Templates;

[Tags("Шаблоны Удобств")]
[Route("api/templates/amenities")]
public class AmenityTemplateController
    (IBaseRepository<AmenityTemplate> repository) : TemplateController<AmenityTemplate>(repository);