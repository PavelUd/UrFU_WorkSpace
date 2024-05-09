using System.Text;
using Newtonsoft.Json;

namespace UrFU_WorkSpace.Helpers;

public class HttpRequestSender
{
    
    private static HttpClient _client = new HttpClient();
    
    public static async Task<HttpResponseMessage> SendPostRequest(Dictionary<string, object> data, string route)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.None);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var responseMessage = await _client.PostAsync(route, content);
        return responseMessage;
    }

    public static async Task<HttpResponseMessage> SentGetRequest( string route)
    {
        return await _client.GetAsync(route);
    }
}