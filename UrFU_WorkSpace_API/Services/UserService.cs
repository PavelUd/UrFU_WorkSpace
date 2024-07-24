using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MimeKit;
using Org.BouncyCastle.Asn1.Cmp;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace UrFU_WorkSpace_API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _codeExpiration = TimeSpan.FromMinutes(10);

    public UserService(IUserRepository userRepository, IConfiguration configuration, IMemoryCache memoryCache)
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