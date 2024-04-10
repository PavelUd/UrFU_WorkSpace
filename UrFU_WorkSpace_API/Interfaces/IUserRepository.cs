using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IUserRepository
{
    public User GetUser(int userId);
    public bool CreateUser(User user);
    public bool Save();
    public IEnumerable<User> GetAllUsers();
}