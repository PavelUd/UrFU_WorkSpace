using UrFU_WorkSpace_API.Helpers;

namespace UrFU_WorkSpace_API.Services;

public interface IWorkspaceComponentService<T>
{
    public Result<None> ValidateComponents(IEnumerable<T> components);
    public IEnumerable<T> GetComponents(int idWorkspace);
}