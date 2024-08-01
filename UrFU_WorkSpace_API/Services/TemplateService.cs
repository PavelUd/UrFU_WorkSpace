using System.Collections;
using AutoMapper;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Services;

public class TemplateService : ITemplateService
{
    private readonly IBaseRepository<Template> _templateRepository;
    private readonly ImageService _imageService;
    private readonly ErrorHandler _errorHandler;
    private readonly IMapper _mapper;

    public TemplateService(IBaseRepository<Template> templateRepository,ImageService imageService, ErrorHandler errorHandler, IMapper mapper)
    {
        _templateRepository = templateRepository;
        _imageService = imageService;
        _errorHandler = errorHandler;
        _mapper = mapper;
    }

    public Result<IEnumerable<Template>> GetAll()
    {
        return Result.Ok().Then(_ => _templateRepository.FindAll().AsEnumerable());
    }

    public Result<None> Delete(int id)
    {
        return GetTemplateById(id).Then(_templateRepository.Delete);
    }

    public Result<int> CreateTemplate(ModifyTemplateDto modifyDto)
    {
        return ConstructTemplate(modifyDto).Then(_templateRepository.Create);

    } 
    
    public Result<Template> GetTemplateById(int id)
    {
        if (!_templateRepository.ExistsById(id))
        {
            return Result.Fail<Template>(_errorHandler.RenderError(ErrorType.TemplateNotFound, 
                new Dictionary<string, string>{ { "value", id.ToString() } }));
        }
        var template = _templateRepository.FindByCondition(x => x.Id == id).FirstOrDefault();
        return Result.Ok<Template>(template);
    }

    private Result<Template> ConstructTemplate(ModifyTemplateDto modifyDto)
    {
        var image = _imageService.ConstructImage(modifyDto.Image, (int)OwnerType.Template);
        var template = _mapper.Map<Template>(modifyDto);
        template.Image = _mapper.Map<TemplateImage>(image);
        return template;
    }
}