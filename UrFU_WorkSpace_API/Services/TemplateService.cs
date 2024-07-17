using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using IModel = UrFU_WorkSpace_API.Models.IModel;

namespace UrFU_WorkSpace_API.Services;

public class TemplateService<T> where T : IModel
{
    private IBaseRepository<T> TemplateRepository;

    public TemplateService(IBaseRepository<T> templateRepository)
    {
        this.TemplateRepository = templateRepository;
    }

    public Result<T> GetTemplateById(int id)
    {
        var template = TemplateRepository.FindByCondition(x => x.Id == id).FirstOrDefault();
        return template == null ? Result.Fail<T>(ErrorHandler.RenderError(ErrorType.TemplateNotFound)) : Result.Ok(template);
    }
}