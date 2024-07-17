using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Helpers;

public static class ErrorHandler
{
    private static readonly Dictionary<ErrorType, Error> Errors = new()
    {
        {ErrorType.WorkspacesNotFound,new Error("Коворкинги не найдены",ErrorType.WorkspacesNotFound, HttpStatusCode.NotFound) },
        {ErrorType.WorkspaceNotFound,new Error("Коворкиг с id {idWorkspace} не найден",ErrorType.WorkspaceNotFound, HttpStatusCode.NotFound) },
        {ErrorType.WorkspaceComponentNotFound,new Error("Компонет коворкинга не найден",ErrorType.WorkspaceComponentNotFound, HttpStatusCode.NotFound) },
        {ErrorType.InvalidPosition,new Error("Позиция на оси {name}, равное {value}, не должна быть меньше 0",ErrorType.InvalidPosition, HttpStatusCode.UnprocessableContent) },
        {ErrorType.InvalidSize,new Error("{name}, равная {value}, не должна быть меньше 1",ErrorType.InvalidSize, HttpStatusCode.UnprocessableContent) },
        {ErrorType.TemplateNotFound,new Error("Шаблон с id {value} не найден",ErrorType.WorkspaceComponentNotFound, HttpStatusCode.NotFound) },
        {ErrorType.InvalidDayOfWeek,new Error("Номер для дня недели не может быть больше 7",ErrorType.WorkspaceComponentNotFound, HttpStatusCode.UnprocessableContent) },
        {ErrorType.IncorrectCountWeekdays,new Error("Количество объектов в Режиме работы дожно быть не более 7",ErrorType.WorkspaceComponentNotFound, HttpStatusCode.UnprocessableContent) },
    };

    public static Error RenderError(ErrorType type, Dictionary<string, string>? args = default)
    {
        if (!Errors.TryGetValue(type, out Error baseError)) 
            return new Error("Неизвестная ошибка");

        if (args != null)
        {
            baseError.Message = FormatError(baseError.Message, args);
        }
        
        return baseError;

    }

    private static string FormatError(string errorTemplate, IReadOnlyDictionary<string, string>? userInputs)
    {
        var regex = new Regex(@"\{(\w+)\}");
            
        var formattedError = regex.Replace(errorTemplate, match =>
        {
            var key = match.Groups[1].Value;
            return userInputs.TryGetValue(key, out string value) ? value : match.Value;
        });
        
        return formattedError;
    }
}