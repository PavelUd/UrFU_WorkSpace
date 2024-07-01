using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class ReservationService : IReservationService
{
    public IReservationRepository Repository;
    private IVerificationCodeService _verificationCodeService;
    
    public ReservationService(IReservationRepository repository, IVerificationCodeService verificationCodeService)
    {
        _verificationCodeService = verificationCodeService;
        Repository = repository;
    }

    public async Task<List<Reservation>> GetReservations(int idWorkspace, DateOnly date)
    {
        return await Repository.GetReservations(idWorkspace, date);
    }
    
    public async Task<List<Reservation>> GetUserReservations(int idUser)
    {
        return await Repository.GetUserReservations(idUser);
    }

    public bool VerifyReservation(string code, int id, int idWorkspace)
    {
        var isOk = _verificationCodeService.GetCodes().Result.Any(x =>x.Code == code.ToUpper() && x.IdWorkspace == idWorkspace);
        if (isOk)
        {
            Repository.ConfirmReservation(id);
        }

        return isOk;
    }
    
    public List<WorkspaceObject> UpdateReservationStatus(TimeOnly start, TimeOnly end, List<Reservation> reservations, List<WorkspaceObject> objects)
    {
        var objs = new List<WorkspaceObject>();
        foreach (var obj in objects)
        {
            var isReserved = reservations.Any(x => x.IdObject == obj.Id && !(x.TimeStart >= end || x.TimeEnd <= start));
            obj.IsReserve = isReserved;
            objs.Add(obj);
        }

        return objs;
    }
    
    public async Task<Reservation> Reserve(int idWorkspace, IFormCollection form)
    {
        var dictionary = new Dictionary<string, object>()
        {
            { "idObject", int.Parse(form["idObject"]) },
            { "idUser", int.Parse(form["idUser"]) },
            { "idWorkspace", idWorkspace},
            { "timeStart", form["timeStart"].ToString() },
            { "timeEnd", form["timeEnd"].ToString() },
            { "date", form["date"].ToString() }
        };

        return await Repository.CreateReservations(dictionary);
    }
}