using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services.Interfaces;

public interface ITemplateService
{
    public Result<IEnumerable<Template>> GetAll();
    public Result<None> Delete(int id);
    public Result<int> CreateTemplate(ModifyTemplateDto modifyDto);
    public Result<Template> GetTemplateById(int id);
    
}