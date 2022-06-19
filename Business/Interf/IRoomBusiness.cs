using CMS.Models;

namespace CMS.Business;
public interface IRoomBusiness
{
    public List<RoomOutDto> getManagerRoom(string userId);

    public List<RoomOutDto> getAllRoom();

    public int modifyRoom(Room room);

    public RoomOutDto addSingleRoom(string userId, AddRoomInDto addRoomInDto);

    public List<RoomPositionOutDto> getAllPositions();

    public int unManageRoom(UnManageInDto unManageInDto);

    public RoomOutDto getSingleRoom(string roomId);

    public List<RoomOutDto> getRoomWithFavorite(string userId);

    public RoomOutDto getSingleRoomWithFavorite(string userId, string roomId);

    // public List<Room> getFreeRooms(DateTime startTime, DateTime endTime);
}