using System.Linq.Expressions;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services.Interfaces;

public interface IBaseProvider<T>
{
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    public IQueryable<T> FindAll();
    public bool ExistsById(int id);
}