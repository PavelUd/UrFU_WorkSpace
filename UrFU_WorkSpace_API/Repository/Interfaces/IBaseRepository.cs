using System.Linq.Expressions;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IBaseRepository<T>
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    int Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    public T Replace(T oldEntity, T entity);
    bool Save();
}