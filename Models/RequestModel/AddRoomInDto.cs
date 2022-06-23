using CMS.CONFIG;

using System.Text.Json.Serialization;
namespace CMS.Models;

public class AddRoomInDto
{
    public string RoomName { get; set; } = null!;

    public string Building { get; set; } = null!;

    [JsonConverter(typeof(FloorConverter))]
    public string Floor { get; set; } = null!;

    public string? RoomDescription { get; set; }

    public int? Capacity { get; set; }

    public string? Image { get; set; }
}