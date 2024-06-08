using System.Net;
using Newtonsoft.Json;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class ReservationRepository : IReservationRepository
{
    private Uri BaseAddress;
    
    public ReservationRepository(string baseApiAddress) 
    {
        BaseAddress = new Uri(baseApiAddress);
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
    
    public async Task<int> CreateReservations(Dictionary<string, object> dictionary)
    {
        var responseMessage = await HttpRequestSender.SendRequest(BaseAddress + "/reservations/reserve", RequestMethod.Post, dictionary);
        responseMessage.EnsureSuccessStatusCode();

        var content = JsonHelper.Deserialize<Reservation>(await responseMessage.Content.ReadAsStringAsync());
        
        return content.IdReservation;
    }
}