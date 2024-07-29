using System.Linq.Expressions;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IBaseRepository<T>
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    public bool ExistsById(int id);
    int Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    public void DeleteRange(IEnumerable<T> reservations);
    bool Save();
}