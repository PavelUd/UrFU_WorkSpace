using Newtonsoft.Json;

namespace UrFU_WorkSpace_API.Helpers;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private readonly string Format = "yyyy-MM-dd";
    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(Format));
    }

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        return DateOnly.ParseExact(reader.Value.ToString(), Format);
    }
}