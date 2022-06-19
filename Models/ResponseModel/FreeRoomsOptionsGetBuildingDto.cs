

namespace CMS.Models;

public class FreeRoomsOptionsGetBuildingDto
{

    public String Value { get; set; }
    public String Label { get; set; }
    public List<FreeRoomsOptionsGetFloorDto> Children { get; set; } = new List<FreeRoomsOptionsGetFloorDto>();
}