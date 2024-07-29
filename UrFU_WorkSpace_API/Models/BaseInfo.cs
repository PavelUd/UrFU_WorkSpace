namespace UrFU_WorkSpace_API.Models;

public class BaseInfo
{
    public string Name { get; set; }

    public string Description { get; set; }

    public double Rating { get; set; }

    public string Address { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
    
    public string Institute { get; set; }

    public int Privacy { get; set; }

    public int IdCreator { get; set; }
}