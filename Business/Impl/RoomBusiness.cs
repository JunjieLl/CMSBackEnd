
using CMS.Models;
using AutoMapper;
using System.Linq.Expressions;

namespace CMS.Business;
public class RoomBusiness : IRoomBusiness
{
    private readonly cmsContext context;

    private readonly IMapper mapper;

    public RoomBusiness(cmsContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public List<RoomOutDto> getManagerRoom(string userId)
    {
        return (from manage in context.Manages
                join room in context.Rooms on manage.RoomId equals room.RoomId
                where manage.UserId == userId
                select new RoomOutDto
                {
                    RoomId = room.RoomId,
                    RoomName = room.RoomName,
                    Building = room.Building,
                    Floor = room.Floor,
                    RoomDescription = room.RoomDescription,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    State = null,
                    Favorite = false
                }).ToList();
    }

    public List<RoomOutDto> getAllRoom()
    {
        var rooms = context.Rooms.ToList();
        List<RoomOutDto> roomOutDtos = new List<RoomOutDto>();
        DateTime dateTime = DateTime.Now;
        rooms.ForEach(r =>
        {
            RoomOutDto roomOutDto = mapper.Map<RoomOutDto>(r);
            roomOutDto.State = "空闲";
            roomOutDto.Favorite = false;
            var activitys = context.Activities.Where(a => a.RoomId.Equals(roomOutDto.RoomId)).ToList();
            foreach (var a in activitys)
            {
                if (a.StartTime <= dateTime && a.StartTime + TimeSpan.FromMinutes(a.Duration) >= dateTime)
                {
                    roomOutDto.State = "忙碌";
                    break;
                }
            }
            roomOutDtos.Add(roomOutDto);
        });
        return roomOutDtos;
    }

    public int modifyRoom(Room room)
    {
        Room modifyRoom = context.Rooms.Single(r => r.RoomId.Equals(room.RoomId));
        if (modifyRoom == null)
        {
            return -1;
        }
        modifyRoom.Building = room.Building;
        modifyRoom.Capacity = room.Capacity;
        modifyRoom.Floor = room.Floor;
        modifyRoom.Image = room.Image;
        modifyRoom.RoomDescription = room.RoomDescription;
        modifyRoom.RoomName = room.RoomName;

        context.SaveChanges();
        return 1;
    }

    public RoomOutDto addSingleRoom(string userId, AddRoomInDto addRoomInDto)
    {
        string? roomId = context.Rooms.Max(r => r.RoomId);
        if (roomId == null)
        {
            roomId = string.Format("{0:D10}", 1);
        }
        else
        {
            roomId = string.Format("{0:D10}", int.Parse(roomId) + 1);
        }
        Room room = mapper.Map<Room>(addRoomInDto);
        room.RoomId = roomId;
        context.Rooms.Add(room);
        Manage manage = new Manage();
        manage.UserId = userId;
        manage.RoomId = roomId;
        context.Manages.Add(manage);
        context.SaveChanges();
        return mapper.Map<RoomOutDto>(room);
    }

    public List<RoomPositionOutDto> getAllPositions()
    {
        var rooms = context.Rooms.ToList();
        List<RoomPositionOutDto> roomPositionOutDtos = new List<RoomPositionOutDto>();
        rooms.ForEach(r => roomPositionOutDtos.Add(mapper.Map<RoomPositionOutDto>(r)));
        return roomPositionOutDtos;
    }

    public int unManageRoom(UnManageInDto unManageInDto)
    {
        var manage = context.Manages.Single(m => m.UserId.Equals(unManageInDto.UserId) &&
        m.RoomId.Equals(unManageInDto.RoomId));
        if (manage == null)
        {
            return -1;
        }

        context.Manages.Remove(manage);
        context.SaveChanges();
        return 1;
    }

    public RoomOutDto getSingleRoom(string roomId)
    {
        try
        {
            Room room = context.Rooms.Single(r => r.RoomId.Equals(roomId));

            RoomOutDto roomOutDto = mapper.Map<RoomOutDto>(room);

            var activities = context.Activities.Where(a => a.RoomId.Equals(roomId));
            DateTime curTime = DateTime.Now;
            foreach (var a in activities)
            {
                if (a.StartTime <= curTime && a.StartTime + TimeSpan.FromMinutes(a.Duration) >= curTime)
                {
                    roomOutDto.State = "忙碌";
                    break;
                }
            }
            return roomOutDto;
        }
        catch
        {
            return null;
        }
    }

    public List<RoomOutDto> getRoomWithFavorite(string userId)
    {
        List<RoomOutDto> roomOutDtos = getAllRoom();
        List<Favorite> favorites = context.Favorites.Where(f => f.UserId.Equals(userId)).ToList();
        roomOutDtos.ForEach(r =>
        {
            if (favorites.Any(f => f.RoomId.Equals(r.RoomId)))
            {
                r.Favorite = true;
            }
        });

        return roomOutDtos;
    }

    public RoomOutDto getSingleRoomWithFavorite(string userId, string roomId)
    {
        RoomOutDto roomOutDto = mapper.Map<RoomOutDto>(context.Rooms.Single(r => r.RoomId.Equals(roomId)));

        var activities = context.Activities.Where(a => a.RoomId.Equals(roomId)).ToList();
        DateTime curTime = DateTime.Now;
        if (activities.Any(a => a.StartTime <= curTime && a.StartTime + TimeSpan.FromMinutes(a.Duration) >= curTime))
        {
            roomOutDto.State = "忙碌";
        }

        var favorites = context.Favorites.Where(f => f.UserId.Equals(userId)).ToList();
        if (favorites.Any(f => f.RoomId.Equals(roomId)))
        {
            roomOutDto.Favorite = true;
        }
        return roomOutDto;
    }

    public List<Room> getFreeRooms(DateTime startTime, DateTime endTime)
    {
        List<Activity> activities = context.Activities.ToList();
        List<RoomOutDto> allRooms = getAllRoom();
        List<Room> rooms = new List<Room>();
        foreach (var roomGetDto in allRooms)
        {
            if (activities.All(a =>
            !a.RoomId.Equals(roomGetDto.RoomId)
            || a.StartTime > endTime
            || startTime > (a.StartTime.AddMinutes(a.Duration))))
            {
                Room room = new Room();
                room.RoomId = roomGetDto.RoomId;
                room.RoomName = roomGetDto.RoomName;
                room.Building = roomGetDto.Building;
                room.Floor = roomGetDto.Floor;
                rooms.Add(room);
            }
        }
        return rooms;
    }

    public FreeRoomOutDto generateRoomGetAllDto(List<Room> rooms)
    {
        List<string> roomIds = rooms.Select(r => r.RoomId).ToList();
        List<FreeRoomsOptionsGetBuildingDto> options = new List<FreeRoomsOptionsGetBuildingDto>();
        FreeRoomOutDto freeRoomOutDto = new FreeRoomOutDto();
        foreach (var room in rooms)
        {
            int i = 0;
            for (i = 0; i < options.Count; ++i)
            {
                if (options[i].Value.Equals(room.Building))
                {
                    int j = 0;
                    for (j = 0; j < options[i].Children.Count; ++j)
                    {
                        if (options[i].Children[j].Value.Equals(room.Floor))
                        {
                            int k = 0;
                            for (k = 0; k < options[i].Children[j].Children.Count; ++k)
                            {
                                if (options[i].Children[j].Children[k].Value.Equals(room.RoomId))
                                {
                                    break;
                                }
                            }
                            if (k == options[i].Children[j].Children.Count)
                            {
                                var newRoom = new FreeRoomsOptionsGetRoomDto();
                                newRoom.Label = room.RoomName;
                                newRoom.Value = room.RoomId;
                                options[i].Children[j].Children.Add(newRoom);
                            }
                            break;
                        }
                    }
                    if (j == options[i].Children.Count)
                    {
                        var newFloor = new FreeRoomsOptionsGetFloorDto();
                        newFloor.Label = room.Floor;
                        newFloor.Value = room.Floor;
                        var newRoom = new FreeRoomsOptionsGetRoomDto();
                        newRoom.Label = room.RoomName;
                        newRoom.Value = room.RoomId;
                        List<FreeRoomsOptionsGetRoomDto> newRooms = new List<FreeRoomsOptionsGetRoomDto>();
                        newRooms.Add(newRoom);
                        newFloor.Children = newRooms;
                        options[i].Children.Add(newFloor);
                    }
                    break;
                }

            }
            if (i == options.Count)
            {
                var newBuilding = new FreeRoomsOptionsGetBuildingDto();
                newBuilding.Value = room.Building;
                newBuilding.Label = room.Building;
                var newFloor = new FreeRoomsOptionsGetFloorDto();
                newFloor.Label = room.Floor;
                newFloor.Value = room.Floor;
                var newRoom = new FreeRoomsOptionsGetRoomDto();
                newRoom.Value = room.RoomId;
                newRoom.Label = room.RoomName;
                List<FreeRoomsOptionsGetRoomDto> newRooms = new List<FreeRoomsOptionsGetRoomDto>();
                newRooms.Add(newRoom);
                newFloor.Children = newRooms;
                List<FreeRoomsOptionsGetFloorDto> newFloors = new List<FreeRoomsOptionsGetFloorDto>();
                newFloors.Add(newFloor);
                newBuilding.Children = newFloors;
                options.Add(newBuilding);

            }
        }
        freeRoomOutDto.Rooms = roomIds;
        freeRoomOutDto.Options = options;
        return freeRoomOutDto;
    }
}