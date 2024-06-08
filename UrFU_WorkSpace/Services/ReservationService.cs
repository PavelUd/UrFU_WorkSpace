using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class ReservationService : IReservationService
{
    public IReservationRepository Repository;
    
    public ReservationService(IReservationRepository repository)
    {
        Repository = repository;
    }

    public async Task<List<Reservation>> GetReservations(int idWorkspace, DateOnly date)
    {
        return await Repository.GetReservations(idWorkspace, date);
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
    
    public async Task<int> Reserve(int idWorkspace, IFormCollection form)
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