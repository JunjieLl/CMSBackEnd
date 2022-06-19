
namespace CMS.Models;

public class FreeRoomOutDto
{

    public List<string> Rooms { get; set; } = new List<string>();

    public List<FreeRoomsOptionsGetBuildingDto> Options { get; set; } = new List<FreeRoomsOptionsGetBuildingDto>();




}