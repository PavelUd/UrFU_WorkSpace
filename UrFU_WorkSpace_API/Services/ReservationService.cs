using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services;

public class ReservationService
{
    private IBaseRepository<Reservation> Repository;
    public ReservationService(IBaseRepository<Reservation> repository)
    {
        Repository = repository;
    }
    
    public Result<Reservation> GetReservationById(int id)
    {
        var result = Repository.FindByCondition(x => x.Id == id).FirstOrDefault();

        var args = new Dictionary<string, string>
        {
            {"id", id.ToString()}
        };
        
        return result ?? Result.Fail<Reservation>(ErrorHandler.RenderError(ErrorType.ReservationNotFound, args));
    }

    public Result<IEnumerable<Reservation>> GetReservations(DateTime date,int idWorkspace, int idUser)
    {
        var reservations = Repository.FindAll();
        if (date != DateTime.MinValue)
            reservations = reservations.Where(x => x.Date == new DateOnly(date.Year, date.Month, date.Day));
        
        if (idWorkspace != 0)
            reservations = reservations.Where(x => x.IdWorkspace == idWorkspace);

        if (idUser != 0)
            reservations = reservations.Where(x => x.IdUser == idUser);

        return Result.Ok<IEnumerable<Reservation>>(reservations);
    }
    
}