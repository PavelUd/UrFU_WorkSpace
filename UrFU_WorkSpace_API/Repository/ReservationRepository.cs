using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Repository;

public class ReservationRepository : IReservationRepository
{
    private UrfuWorkSpaceContext _context;
    
    public ReservationRepository(UrfuWorkSpaceContext context)
    {
        _context = context;
    }
    public IEnumerable<Reservation> GetUserReservations(int idUser)
    {
        return _context.Reservations.Where(reservation => reservation.IdUser == idUser);
    }
}