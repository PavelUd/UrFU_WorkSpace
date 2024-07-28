using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IWorkspaceComponent : IModel
{
    public int IdWorkspace { get; set; }
}