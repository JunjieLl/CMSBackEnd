using CMS.CONFIG;
using System.Text.Json.Serialization;
namespace CMS.Models;

public class FreeRoomInFto
{
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime StartTime { get; set; }

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime EndTime { get; set; }
}