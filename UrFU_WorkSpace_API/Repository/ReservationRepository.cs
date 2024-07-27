using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Repository;

public class ReservationRepository(UrfuWorkSpaceContext context) : BaseRepository<Reservation>(context), IReservationRepository
{
    public bool Confirm(int id, bool confirm)
    {
        var updatedRowsCount = context.Reservations.Where(u => u.Id == id)
            .ExecuteUpdate(b =>
                b.SetProperty(u => u.IsConfirmed, confirm)
            );
        return confirm && updatedRowsCount == 1;
    }
}