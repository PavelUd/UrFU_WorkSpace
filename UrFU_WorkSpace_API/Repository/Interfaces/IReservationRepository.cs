using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IReservationRepository : IBaseRepository<Reservation>
{
    public bool Confirm(int id, bool confirm);
}