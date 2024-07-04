using System.Linq.Expressions;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IUserService
{
    public AuthenticateResponse Authenticate(AuthenticateRequest authenticate);
    public IEnumerable<User> GetUsersByCondition(Expression<Func<User, bool>> expression);
    public void UpdateAccessLevel(int idUser, int newAccessLevel);

    public Task<AuthenticateResponse> Register(User user);
    public IEnumerable<User> GetAllUsers();
}