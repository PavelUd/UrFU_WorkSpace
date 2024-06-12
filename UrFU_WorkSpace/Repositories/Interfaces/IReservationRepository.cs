using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IReservationRepository
{
    public Task<List<Reservation>> GetReservations(int idWorkspace, DateOnly date);
    public Task<Reservation> CreateReservations(Dictionary<string, object> dictionary);
};