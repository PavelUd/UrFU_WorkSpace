
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _repository;
    
    public ReservationService(IReservationRepository repository)
    {
        _repository = repository;
    }

    public  Result<Reservation> GetReservation(int id, string token)
    {
        return _repository.GetReservation(id, token).Result;
    }
    
    public async Task<Result<List<Reservation>>> GetReservations(int idWorkspace, string token)
    {
        return await _repository.GetReservations(null, idWorkspace,false, token);
    }

    public async Task<Result<List<Reservation>>> GetReservations(string token, bool isFull = false)
    {
        var idUser = JwtTokenDecoder.GetUserId(token);
        return await _repository.GetReservations(idUser, null, isFull, token);
    }
    
    public bool VerifyReservation(string code, int id, int idWorkspace)
    {
        var isOk = true;
        return isOk;
    }
    
    
    public async Task<Result<int>> Reserve(int idWorkspace, IFormCollection form, string token)
    {
        var dictionary = new Dictionary<string, object>()
        {
            { "idObject", int.Parse(form["idObject"]) },
            { "idUser", int.Parse(form["idUser"]) },
            { "idWorkspace", idWorkspace},
            { "timeStart", form["timeStart"].ToString() },
            { "timeEnd", form["timeEnd"].ToString() },
            { "date", form["date"].ToString() }
        };

        return await _repository.CreateReservations(dictionary, token);
    }
    
    
}