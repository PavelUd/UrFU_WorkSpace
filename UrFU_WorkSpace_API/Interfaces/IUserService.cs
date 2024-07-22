using System.Linq.Expressions;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IUserService
{
    public Result<string> Authenticate(AuthenticateRequest authenticate);
    public IQueryable<User> GetUsersByCondition(Expression<Func<User, bool>> expression);
    public Result<None> Confirm(string login, int code);
    public Result<string> Register(User user);
    public IEnumerable<User> GetAllUsers();
    public void SaveUserInfo(User user, int code);
    public Result<None> SendConfirmEmail(int code, string email);
}