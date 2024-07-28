using System.Linq.Expressions;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IUserService
{
    public IQueryable<User> GetUsersByCondition(Expression<Func<User, bool>> expression);
    public IEnumerable<User> GetAllUsers();
    public Result<int> CreateUser(User user);
}