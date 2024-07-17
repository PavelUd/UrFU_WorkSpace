using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;

namespace UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

public abstract class WorkspaceComponentService<T> : IWorkspaceComponentService<T>  where T : IWorkspaceComponent
{
    
    protected IBaseRepository<T> Repository;

    protected WorkspaceComponentService(IBaseRepository<T> repository)
    {
        Repository = repository;
    }

    public virtual Result<None> ValidateComponents(IEnumerable<T> components)
    {
        return Result.Ok();
    }

    public  Result<IEnumerable<T>> GetComponents(int idWorkspace)
    {
        var message = ErrorHandler.RenderError(ErrorType.WorkspaceComponentNotFound);
        var entities = Repository.FindByCondition(x => x.IdWorkspace == idWorkspace);
        return !entities.Any() ?
            Result.Fail<IEnumerable<T>>(message) 
            : Result.Ok(entities.AsEnumerable());
    }
    
    protected Result<None> ValidateParam<TParam>(bool condition,ErrorType errorType, TParam value = default, string name = "")
    {
        var args = new Dictionary<string, string?>
        {
            { "value", value == null ? "" : value.ToString() },
            { "name", name }
        };
        return condition
            ? Result.Ok()
            : Result.Fail<None>(ErrorHandler.RenderError(errorType, args));
    }
}