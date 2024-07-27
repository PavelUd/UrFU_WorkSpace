using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services;

public class TemplateService<T> where T : IModel
{
    private readonly IBaseRepository<T> _templateRepository;
    private readonly ErrorHandler _errorHandler;

    public TemplateService(IBaseRepository<T> templateRepository, ErrorHandler errorHandler)
    {
        _templateRepository = templateRepository;
        _errorHandler = errorHandler;
    }

    public Result<T> GetTemplateById(int id)
    {
        var template = _templateRepository.FindByCondition(x => x.Id == id).FirstOrDefault();
        return template == null
            ? Result.Fail<T>(_errorHandler.RenderError(ErrorType.TemplateNotFound))
            : Result.Ok(template);
    }
}