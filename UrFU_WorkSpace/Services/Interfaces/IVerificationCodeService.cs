using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IVerificationCodeService
{
    public  Task<VerificationCode> UpdateCode(int idWorkspace, int idCode);
    public Task<bool> AddCode(int idWorkspace);
    public Task<IEnumerable<VerificationCode>> GetCodes(int idUser = 0);
}