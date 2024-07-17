using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;

namespace UrFU_WorkSpace_API.Services;

public interface IWorkspaceComponentService<T>
{
    public Result<None> ValidateComponents(IEnumerable<T> components);
    public  Result<IEnumerable<T>> GetComponents(int idWorkspace);
}