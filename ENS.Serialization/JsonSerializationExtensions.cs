using Newtonsoft.Json;

namespace ENS.Serialization;

public static class JsonSerializationExtensions
{
    public static string ToJson(this object entity) => JsonConvert.SerializeObject(entity);

    public static T? ToType<T>(this string str) => JsonConvert.DeserializeObject<T>(str);
}