using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class VerificationCodeService : IVerificationCodeService
{
    private IVerificationCodeRepository Repository;
    
    public VerificationCodeService(IVerificationCodeRepository repository) 
    {
        Repository = repository;
    }

    private VerificationCode GenerateCode(int idWorkspace, int id = 0)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var code = new string(
            Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

        return new VerificationCode()
        {
            Id = id,
            Code = code,
            Date = DateOnly.FromDateTime(DateTime.Now),
            IdWorkspace = idWorkspace
        };
    }
    
    public async Task<IEnumerable<VerificationCode>> GetCodes(int idUser = 0)
    {
        return await Repository.GetVerificationCodes(idUser);
    }
    
    public async Task<bool> AddCode(int idWorkspace)
    {
        var code = GenerateCode(idWorkspace);
        return await Repository.AddVerificationCode(code);
    }

    public async Task<VerificationCode> UpdateCode(int idWorkspace, int idCode)
    {
        var code = GenerateCode(idWorkspace, idCode);
       return await Repository.UpdateVerificationCode(code);
    }
}