using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers.Templates;

[Route("api/templates/amenities")]
public class AmenityTemplateController(IAmenityTemplateRepository repository) : TemplateController<AmenityTemplate>(repository);