using Newtonsoft.Json;

namespace UrFU_WorkSpace.Helpers;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private readonly string[] Formats = { "HH:mm", "h:mm tt", "HH:mm:ss" };
    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(Formats[0]));
    }

    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        return TimeOnly.ParseExact(reader.Value.ToString(), Formats);
    }
}