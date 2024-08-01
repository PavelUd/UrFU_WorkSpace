using System.ComponentModel.DataAnnotations;

namespace UrFU_WorkSpace_API.Helpers;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        switch (value)
        {
            case List<IFormFile> fileList:
                var isValid = fileList.Select(IsValidFile).All(r => r);
                return isValid ? ValidationResult.Success : new ValidationResult(GetErrorMessage());
                break;
            case IFormFile file:
                return IsValidFile(file) ? ValidationResult.Success : new ValidationResult(GetErrorMessage());
            default:
                return ValidationResult.Success;
        }
        
    }

    private bool IsValidFile(IFormFile? file)
    {
        var flag = true;
        if (file != null)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!_extensions.Contains(extension.ToLower()))
            {
                flag = false;
            }
        }
        return flag;
    }
    
    private static string GetErrorMessage()
    {
        return $"Неверный тип изображения";
    }
}