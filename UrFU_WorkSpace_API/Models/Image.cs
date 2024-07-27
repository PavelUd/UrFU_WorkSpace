using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace UrFU_WorkSpace_API.Models;

public class WorkspaceImage : Image
{
}

public class ObjectImage : Image
{
}

public class AmenityImage : Image
{
}

[Table("images")]
public class Image : IModel
{
    [JsonIgnore] [Column("type_owner")] public int TypeOwner { get; set; }

    [Column("url")] public string Url { get; set; }

    [JsonIgnore] [Column("owner_id")] public int IdOwner { get; set; }

    [Column("image_id")] public int Id { get; set; }
}