using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Repository;

public class UserRepository : IUserRepository
{
    private UrfuWorkSpaceContext _context;
    public UserRepository(UrfuWorkSpaceContext context)
    {
        _context = context;
    }
    public User GetUser(int userId)
    {
        return _context.Users.Find(userId);
    }

    public bool AddUser(User user)
    {
        _context.Users.Add(user);
        return Save();
    }
    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _context.Users;
    }
}