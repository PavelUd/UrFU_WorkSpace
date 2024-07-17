using System.ComponentModel.DataAnnotations;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Dto;

public class ModifyWorkspaceDto
{
    [Required]
     public string Name { get; set; }
     
     public string Description { get; set; }
    
     [Required]
     public string Institute { get; set; }
     
     [Required]
     public IEnumerable<WorkspaceObject?> Objects { get; set;}
     public IEnumerable<WorkspaceAmenity?> Amenities { get; set;}
    
     [Required]
     public IEnumerable<WorkspaceWeekday?> OperationMode { get; set;}
    public IEnumerable<IFormFile> ImageFiles { get; set; } 
    
    
    [Required]
    public string Address { get; set; }
    
    public int Privacy { get; set; }

    [Required]
    public int IdCreator { get; set; }
}