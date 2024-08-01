using System.ComponentModel.DataAnnotations;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;

namespace UrFU_WorkSpace_API.Dto;

public class ModifyTemplateDto
{
    [Required]
    public TemplateCategory Category { get; set; }

    [Required]
    public string Name { get; set; }
    
    [Display(Name = "Image")]
    [Required(ErrorMessage = "Добавте фотографию")]
    [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" })]
    public IFormFile Image { get; set; }
}