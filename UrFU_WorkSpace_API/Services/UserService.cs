using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services;

public class UserService : IUserService
{
    private readonly TimeSpan _codeExpiration = TimeSpan.FromMinutes(10);
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;
    private readonly IBaseRepository<User> _userRepository;

    public UserService(IBaseRepository<User> userRepository, IConfiguration configuration, IMemoryCache memoryCache)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _memoryCache = memoryCache;
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.FindAll();
    }

    public IQueryable<User> GetUsersByCondition(Expression<Func<User, bool>> expression)
    {
        return _userRepository.FindByCondition(expression);
    }

    public Result<int> CreateUser(User user)
    {
        return _userRepository.Create(user);
    }
}