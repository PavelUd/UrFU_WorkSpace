using System.Net;
using System.Text.Json.Serialization;
using UrFU_WorkSpace_API.Enums;

namespace UrFU_WorkSpace_API.Models;

public class Error
{
    public Error(string message = "", ErrorType code = ErrorType.ServerError,
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
    {
        Message = message;
        HttpStatusCode = httpStatusCode;
        Code = code;
    }

    public HttpStatusCode HttpStatusCode { get; set; }

    public ErrorType Code { get; set; }
    public string Message { get; set; }

    public Error? SubError { get; set; }
}