using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IReservationService
{
    public List<WorkspaceObject> UpdateReservationStatus(TimeOnly start, TimeOnly end, List<Reservation> reservations,
        List<WorkspaceObject> objects);

    public Task<Reservation> Reserve(int idWorkspace, IFormCollection form);

    public Task<List<Reservation>> GetReservations(int idWorkspace, DateOnly date);
    public bool VerifyReservation(string code, int id, int idWorkspace);
    public Task<List<Reservation>> GetUserReservations(int idUser);

}