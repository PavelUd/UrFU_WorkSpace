using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Helpers;

public class ErrorHandler
{
    private readonly Dictionary<ErrorType, Error> _errors = new();
    private readonly ILogger<ErrorHandler> _logger;

    public ErrorHandler(string jsonFilePath, ILogger<ErrorHandler> logger)
    {
        _logger = logger;
        LoadErrorsFromJson(jsonFilePath);
    }
    
    private void LoadErrorsFromJson(string jsonFilePath)
    {
        try
        {
            var json = File.ReadAllText(jsonFilePath);
            var errorList = JsonConvert.DeserializeObject<List<Error>>(json);

            foreach (var error in errorList)
            {
                _errors[error.Code] = error;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading errors from JSON");
        }
    }

    public Error RenderError(ErrorType type, Dictionary<string, string>? args = default)
    {
        if (!_errors.TryGetValue(type, out var baseError))
            return new Error("Неизвестная ошибка");

        if (args != null) baseError.Message = FormatError(baseError.Message, args);

        return baseError;
    }

    private static string FormatError(string errorTemplate, IReadOnlyDictionary<string, string>? userInputs)
    {
        var regex = new Regex(@"\{(\w+)\}");

        var formattedError = regex.Replace(errorTemplate, match =>
        {
            var key = match.Groups[1].Value;
            return userInputs.TryGetValue(key, out var value) ? value : match.Value;
        });

        return formattedError;
    }
}