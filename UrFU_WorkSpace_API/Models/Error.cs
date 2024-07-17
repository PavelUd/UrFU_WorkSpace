using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;
using UrFU_WorkSpace_API.Enums;

namespace UrFU_WorkSpace_API.Models;

public class Error
{
    [JsonIgnore]
    public HttpStatusCode Status { get; set; }
    
    public ErrorType Code { get; set; }
    public string Message { get; set; }

    public Error? SubError { get; set; }

    public Error(string message = "",ErrorType code = ErrorType.ServerError, HttpStatusCode status = HttpStatusCode.InternalServerError)
    {
        Message = message;
        Status = status;
        Code = code;
    }
}