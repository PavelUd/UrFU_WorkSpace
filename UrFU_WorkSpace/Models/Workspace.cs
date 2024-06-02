using System.Net;
using Newtonsoft.Json;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;

namespace UrFU_WorkSpace.Models;

public class Workspace
{
    public static Uri baseAdress;
    public int Id { get; set; }
    public string Name { get; set; }

    public IEnumerable<Review> Reviews;
    public string Description { get; set; }
    
    public double Rating { get; set; }
    
    public string Address { get; set; }
    
    public string Institute { get; set; }
    
    public int Privacy { get; set; }
    
    public int IdCreator { get; set; }
    
    public IEnumerable<WorkspaceImage> Images { get; set; }
    public IEnumerable<UrFU_WorkSpace_API.Models.WorkspaceObject> Objects { get; set; }
    public IEnumerable<WorkspaceWeekday> OperationMode { get; set; }
    public IEnumerable<WorkspaceAmenity> Amenities { get; set; }
}