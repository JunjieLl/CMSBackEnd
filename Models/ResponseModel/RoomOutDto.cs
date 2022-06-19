
namespace CMS.Models;

public class RoomOutDto
{

    public string RoomId { get; set; } = null!;

    public string RoomName { get; set; } = null!;

    public string Building { get; set; } = null!;

    public string Floor { get; set; } = null!;

    public string? RoomDescription { get; set; }

    public int? Capacity { get; set; }

    public string? Image { get; set; }

    public string? State { get; set; } = "空闲";

    public bool Favorite { get; set; } = false;
}