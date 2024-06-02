using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IReservationService
{
    public List<WorkspaceObject> UpdateReservationStatus(TimeOnly start, TimeOnly end, List<Reservation> reservations,
        List<WorkspaceObject> objects);

    public Task<int> Reserve(int idWorkspace, IFormCollection form);

    public Task<List<Reservation>> GetReservations(int idWorkspace, DateOnly date);

}