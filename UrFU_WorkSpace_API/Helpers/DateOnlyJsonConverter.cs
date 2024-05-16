using System.Text.Json;
using System.Text.Json.Serialization;

namespace UrFU_WorkSpace_API.Helpers;

public class DateOnlyJsonConverter: JsonConverter<DateOnly>
{
    private readonly string Format = "yyyy-MM-dd";
    
    public override void Write(Utf8JsonWriter writer, DateOnly date, JsonSerializerOptions options)
    {
        writer.WriteStringValue(date.ToString(Format));
    }
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.ParseExact(reader.GetString(), Format);
    }
}