using System.Net;
using Newtonsoft.Json;
using UrFU_WorkSpace_API.Enums;

namespace UrFU_WorkSpace_API.Models;

public class Error
{
    public Error(string message = "", ErrorType type = ErrorType.ServerError,
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
    {
        Message = message;
        HttpStatusCode = httpStatusCode;
        Type = type;
    }

    [JsonIgnore]
    public HttpStatusCode HttpStatusCode { get; set; }

    public ErrorType Type { get; set; }
    public string Message { get; set; }

    public Error? SubError { get; set; }
}