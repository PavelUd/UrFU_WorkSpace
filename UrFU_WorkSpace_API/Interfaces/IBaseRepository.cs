using System.Linq.Expressions;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IBaseRepository<T>
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);

    bool Save();
}