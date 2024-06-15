using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Repositories;

public class ReservationRepository : IReservationRepository
{
    private Uri BaseAddress;
    
    public ReservationRepository(string baseApiAddress) 
    {
        BaseAddress = new Uri(baseApiAddress);
    }

    public async Task<List<Reservation>> GetUserReservations(int idUser)
    {
        var route = BaseAddress + "/reservations?idUser=" + idUser;
        var responseMessage = HttpRequestSender.SendRequest(route, RequestMethod.Get).Result;
        responseMessage.EnsureSuccessStatusCode();
        
        var data = await responseMessage.Content.ReadAsStringAsync();
        return JsonHelper.Deserialize<List<Reservation>>(data);
    }
    
    public async Task<List<Reservation>> GetReservations(int idWorkspace, DateOnly date)
    {
        
        var jsonDate =  JsonHelper.Serialize(date).Replace("\"", "");
        var route = BaseAddress + "/reservations?idWorkspace=" + idWorkspace + "&date=" + jsonDate;
        var responseMessage = HttpRequestSender.SendRequest(route, RequestMethod.Get).Result;
        responseMessage.EnsureSuccessStatusCode();
        
        var data = await responseMessage.Content.ReadAsStringAsync();
            

        return JsonHelper.Deserialize<List<Reservation>>(data);
    }

    public async Task ConfirmReservation(int idReservation)
    {
        await HttpRequestSender.SendRequest(BaseAddress + "/reservations/" + idReservation + "/confirm", RequestMethod.Patch);
    }
    
    public async Task<Reservation> CreateReservations(Dictionary<string, object> dictionary)
    {
        var responseMessage = await HttpRequestSender.SendRequest(BaseAddress + "/reservations/reserve", RequestMethod.Post, dictionary);
        responseMessage.EnsureSuccessStatusCode();

        var content = JsonHelper.Deserialize<Reservation>(await responseMessage.Content.ReadAsStringAsync());
        
        return content;
    }
}