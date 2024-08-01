using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Repositories.Interfaces;

namespace UrFU_WorkSpace.Repositories;

public class ReservationRepository : IReservationRepository
{
    private Uri BaseAddress;
    
    public ReservationRepository(string baseApiAddress) 
    {
        BaseAddress = new Uri(baseApiAddress);
        
    }

    public async Task<Result<Reservation>> GetReservation(int id, string token)
    {
        var route = BaseAddress + $"/reservations/{id}";
        return await HttpRequestSender.HandleJsonRequest<Reservation>(route, HttpMethod.Get, token);
    }
    
    
    public async Task<Result<List<Reservation>>> GetReservations(int? idUser, int? idWorkspace,bool isFull, string token = "")
    {
        var route = BaseAddress + $"/reservations";
        var queryParams = new List<string>();

        foreach (var (value, name) in new[] { (idWorkspace, "idWorkspace"), (idUser, "idUser") })
        {
            if (value.HasValue)
            {
                queryParams.Add($"{name}={value.Value.ToString()}");
            }
        }

        if (isFull)
        {
            queryParams.Add($"{nameof(isFull)}={isFull.ToString().ToLower()}");
        }

        if (queryParams.Any())
            route += "?" + string.Join("&", queryParams);

        return await HttpRequestSender.HandleJsonRequest<List<Reservation>>(route, HttpMethod.Get, token);
    }
    
/*    public async Task ConfirmReservation(int idReservation)
    {
        await HttpRequestSender.SendRequest(BaseAddress + "/reservations/" + idReservation + "/confirm", RequestMethod.Patch);
    }
    
*/    public async Task<Result<int>> CreateReservations(Dictionary<string, object> dictionary, string token)
    {
        return await HttpRequestSender.HandleJsonRequest<int, Dictionary<string, object>>(BaseAddress + "/reservations", HttpMethod.Post, dictionary, token);
   }
 }