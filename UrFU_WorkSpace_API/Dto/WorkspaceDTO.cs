using System.ComponentModel.DataAnnotations;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Dto;

public class WorkspaceDTO
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public double Rating { get; set; }
    
    public string Address { get; set; }
    
    public string Institute { get; set; }
    
    public int Privacy { get; set; }
    
    public int IdCreator { get; set; }
    public IEnumerable<WorkspaceImage> Images { get; set; }
    public IEnumerable<WorkspaceObject> Objects { get; set; }
    public IEnumerable<WorkspaceWeekday> OperationMode { get; set; }
    
    public IEnumerable<WorkspaceAmenity> Amenities { get; set; }
}