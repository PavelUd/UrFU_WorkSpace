using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using UrFU_WorkSpace.Models;

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
    
    public static async Task<HttpResponseMessage> SendRequest<T>(string route, HttpMethod  method, T? data, string? token)
    {
        var client = GetClient();
        var request = new HttpRequestMessage(method, route);
        if (token != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        if (data == null || method == HttpMethod.Get) 
            return await client.SendAsync(request);
        
        var json = JsonConvert.SerializeObject(data, Formatting.None);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        request.Content = content;
        
        return await client.SendAsync(request);
    }

    public static async Task<Result<T>> HandleJsonRequest<T>(string route, HttpMethod method)
    {
        return await HandleJsonRequest<T, object>(route, method, null, null);
    }
    public static async Task<Result<T>> HandleJsonRequest<T>(string route, HttpMethod method, string token)
    {
        return await HandleJsonRequest<T, object>(route, method, null, token);
    }
    
    public static async Task<Result<T>> HandleJsonRequest<T, TS>(string route, HttpMethod method, TS? dictionary, string? token)
    {
        var responseMessage = SendRequest(route,method, dictionary, token).Result;
        var content = await responseMessage.Content.ReadAsStringAsync();
        if (!responseMessage.IsSuccessStatusCode)
        {
            var errorResult = Result.Fail<T>(JsonHelper.Deserialize<Error>(content));
            errorResult.Error.Code = (int)responseMessage.StatusCode;
            return errorResult;
        }

        return JsonHelper.Deserialize<T>(content);
    }
    
    public static async Task<HttpResponseMessage>  SendMultiPart()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5260/api/oauth/token")
        {
            Headers =
            {
                Accept = { new MediaTypeWithQualityHeaderValue("text/plain") },
            },
            Content = new MultipartFormDataContent
            {
                { new StringContent("password"), "GrantType" },
                { new StringContent("ghg"), "Login" },
                { new StringContent("sss"), "Password" },
                { new StringContent(""), "Code" },
            },
        };
        
        var response = await GetClient().SendAsync(request);
        return response;
    }
}