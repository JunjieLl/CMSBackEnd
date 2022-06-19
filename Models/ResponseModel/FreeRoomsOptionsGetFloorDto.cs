namespace CMS.Models;

public class FreeRoomsOptionsGetFloorDto
{

    public String Value { get; set; }
    public String Label { get; set; }

    public List<FreeRoomsOptionsGetRoomDto> Children { get; set; } = new List<FreeRoomsOptionsGetRoomDto>();
}