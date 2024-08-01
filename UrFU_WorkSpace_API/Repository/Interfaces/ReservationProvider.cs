using System.Linq.Expressions;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Repository;

public interface IReservationProvider
{
    public IEnumerable<Reservation> GetWorkspaceReservationByTimeSlot(int idWorkspace, DateOnly date,
        TimeOnly timeStart, TimeOnly timeEnd);
}