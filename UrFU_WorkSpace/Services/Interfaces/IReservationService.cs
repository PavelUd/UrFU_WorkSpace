using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IReservationService
{
    public Task<Result<List<Reservation>>> GetReservations(int idUser, string token);
    public Task<Result<List<Reservation>>> GetReservations(string token, bool isFull);
    public Task<Result<bool>> Confirm(string strLat, string strLog, string token);
    public Task<Result<None>> Delete(int id, string token);
    public Result<Reservation> GetReservation(int id, string token);

    public Task<Result<int>> Reserve(int idWorkspace, IFormCollection form, string token);
}