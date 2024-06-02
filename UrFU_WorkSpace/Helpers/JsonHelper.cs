using Newtonsoft.Json;
using UrFU_WorkSpace.Helpers;

public static class JsonHelper
{
    public static T Deserialize<T>(string json)
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new TimeOnlyJsonConverter());
        settings.Converters.Add(new DateOnlyJsonConverter());
        return JsonConvert.DeserializeObject<T>(json, settings) ?? Activator.CreateInstance<T>();
    }
    
    public static string Serialize<T>(T obj)
    {
        var settings = new JsonSerializerSettings()
            {DateFormatString = "yyyy-MM-dd" };
        
        settings.Converters.Add(new TimeOnlyJsonConverter());
        settings.Converters.Add(new DateOnlyJsonConverter());
        return JsonConvert.SerializeObject(obj, settings);
    }
}