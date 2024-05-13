using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Interfaces;

namespace UrFU_WorkSpace_API.Repository;

public class BaseRepository<T> : IBaseRepository<T>  where T : class
{
    protected UrfuWorkSpaceContext _context;

    public BaseRepository(UrfuWorkSpaceContext context)
    {
        _context = context;
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression).AsNoTracking();
    }
    
    public IQueryable<T> FindAll()
    {
        return _context.Set<T>().AsNoTracking();
    }

    public void Create(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    
    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}