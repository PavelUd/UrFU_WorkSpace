using System.Text;
using Newtonsoft.Json;
using UrFU_WorkSpace.enums;

namespace UrFU_WorkSpace.Helpers;

public static class HttpRequestSender
{
    private static HttpClient GetClient()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback +=
            (sender, certificate, chain, errors) => true;
        return new HttpClient(handler);
    }

    private delegate Task<HttpResponseMessage> RequestDelegate(string route, HttpContent? content);
    private static RequestDelegate RequestMethodInvokerFactory(RequestMethod method)
    {
        var client = GetClient();
        return method switch
        {
            RequestMethod.Get => async (route, _) => await client.GetAsync(route),
            RequestMethod.Post => async (route, content) => await client.PostAsync(route, content),
            RequestMethod.Put => async (route, content) => await client.PutAsync(route, content),
            RequestMethod.Patch => async (route, content) => await client.PatchAsync(route, content),
            _ => throw new ArgumentException("Invalid request method")
        };
    }
    
    public static async Task<HttpResponseMessage> SendRequest(string route, RequestMethod method, Dictionary<string, object>? data = null)
    {
        if (data == null || method == RequestMethod.Get) 
            return await RequestMethodInvokerFactory(method)(route, null);
        
        var json = JsonConvert.SerializeObject(data, Formatting.None);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        return await RequestMethodInvokerFactory(method)(route, content);
    }
}