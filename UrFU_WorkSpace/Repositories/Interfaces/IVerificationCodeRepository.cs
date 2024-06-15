using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Repositories.Interfaces;

public interface IVerificationCodeRepository
{
    public Task<VerificationCode> UpdateVerificationCode(VerificationCode code);

    public Task<IEnumerable<VerificationCode>> GetVerificationCodes(int idUser);
    
    public Task<bool> AddVerificationCode(VerificationCode code);
}