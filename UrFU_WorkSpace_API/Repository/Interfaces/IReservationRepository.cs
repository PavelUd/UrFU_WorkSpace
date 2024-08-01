using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Repository;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IReservationRepository : IBaseRepository<Reservation>, IReservationProvider
{
    public IEnumerable<FullReservation> IncludeFullInfo(IEnumerable<Reservation> reservations);
    public FullReservation IncludeFullInfo(Reservation reservation);
    public bool Confirm(int id, bool confirm);
}