using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class, IModel
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

    public int Create(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
        return entity.Id;
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
    }

    public T Replace(T oldEntity, T entity)
    {
        _context.Set<T>().Remove(oldEntity);
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        _context.SaveChanges();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }

    public void DeleteRange(IEnumerable<T> reservations)
    {
        _context.Set<T>().RemoveRange(reservations);
        _context.SaveChanges();
    }
}