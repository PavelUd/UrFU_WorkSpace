using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Repositories.Interfaces;

public interface IReservationRepository
{
    public Task<Result<List<Reservation>>> GetReservations(int? idUser, int? idWorkspace, bool isFull,
        string token = "");
    public Task<Result<int>> CreateReservations(Dictionary<string, object> dictionary, string token);
    public Task<Result<Reservation>> GetReservation(int id, string token);
}