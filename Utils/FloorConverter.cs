
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CMS.CONFIG;

public class FloorConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int value = -1;
        string res = "";
        try
        {
            value = reader.GetInt32();
            res = value.ToString();
        }
        catch
        {
            res = reader.GetString();
        }
        return res;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}