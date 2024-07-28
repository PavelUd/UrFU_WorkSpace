using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;

namespace UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

public abstract class WorkspaceComponentService<T> : IWorkspaceComponentService<T> where T : IWorkspaceComponent
{
    private readonly IBaseRepository<T> _repository;
    private readonly ErrorHandler _errorHandler;

    protected WorkspaceComponentService(IBaseRepository<T> repository, ErrorHandler errorHandler)
    {
        _repository = repository;
        _errorHandler = errorHandler;
    }

    public abstract Result<None> ValidateComponents(IEnumerable<T> components);

    public Result<IEnumerable<T>> GetComponents(int idWorkspace)
    {
        var message = _errorHandler.RenderError(ErrorType.WorkspaceComponentNotFound);
        var entities = _repository.FindByCondition(x => x.IdWorkspace == idWorkspace);
        return !entities.Any()
            ? Result.Fail<IEnumerable<T>>(message)
            : Result.Ok(entities.AsEnumerable());
    }

    protected Result<None> ValidateParam<TParam>(bool condition, ErrorType errorType, TParam value = default,
        string name = "")
    {
        var args = new Dictionary<string, string?>
        {
            { "value", value == null ? "" : value.ToString() },
            { "name", name }
        };
        return condition
            ? Result.Ok()
            : Result.Fail<None>(_errorHandler.RenderError(errorType, args));
    }
}