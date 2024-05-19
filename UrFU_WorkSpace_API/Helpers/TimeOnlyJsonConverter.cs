using System.Text.Json;
using System.Text.Json.Serialization;

namespace UrFU_WorkSpace_API.Helpers;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private readonly string[] Formats = { "HH:mm", "h:mm tt", "HH:mm:ss" };
    
    public override void Write(Utf8JsonWriter writer, TimeOnly date, JsonSerializerOptions options)
    {
        writer.WriteStringValue(date.ToString(Formats[0]));
    }
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return TimeOnly.ParseExact(reader.GetString(), Formats);
    }
}