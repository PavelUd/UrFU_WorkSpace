using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Dto;
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

    public IEnumerable<Reservation> GetWorkspaceReservationByTimeSlot(int idWorkspace, DateOnly date, TimeOnly timeStart, TimeOnly timeEnd)
    {
        return FindByCondition(r => r.Date == date 
                                    && r.IdWorkspace == idWorkspace 
                                    && (r.TimeStart >= timeStart  || timeStart == TimeOnly.MinValue)
                                    && (r.TimeEnd <= timeEnd || timeEnd == TimeOnly.MinValue));
    }

    public IEnumerable<FullReservation> IncludeFullInfo(IEnumerable<Reservation> reservations)
    {
        return reservations
            .Join(_context.Workspaces, r => r.IdWorkspace, w => w.Id, (r, w) => new { Reservation = r, Workspace = w })
            .Join(_context.WorkspaceObjects, rw => rw.Reservation.IdObject, o => o.Id,
                (rw, o) => new { rw.Reservation, rw.Workspace, WorkspaceObject = o })
            .Select(r => new FullReservation(r.Reservation, r.Workspace, r.WorkspaceObject));
    }
    
    public FullReservation IncludeFullInfo(Reservation reservation)
    {
        return IncludeFullInfo(new List<Reservation>() { reservation }).First();
    }
}