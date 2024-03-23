using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IUserRepository
{
    public User GetUser(int userId);

    public IEnumerable<User> GetAllUsers();
}