using Microsoft.AspNetCore.Mvc;
using CMS.Models;
using CMS.Business;


namespace CMS.Controllers;


[ApiController]
[Route("api/Room")]
public class RoomController : ControllerBase
{
    private readonly IRoomBusiness roomBusiness;

    public RoomController(IRoomBusiness roomBusiness)
    {
        this.roomBusiness = roomBusiness;
    }

    [HttpGet("{userId}")]
    public ActionResult<List<RoomOutDto>> getManagerRoom(string userId)
    {
        return roomBusiness.getManagerRoom(userId);
    }

    [HttpGet]
    public ActionResult<List<RoomOutDto>> getAllRoom()
    {
        return roomBusiness.getAllRoom();
    }

    [HttpPut]
    public IActionResult modifyRoom([FromBody] Room room)
    {
        if (roomBusiness.modifyRoom(room) == -1)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPost("{userId}")]
    public IActionResult addSingleRoom([FromRoute] string userId, [FromBody] AddRoomInDto addRoomInDto)
    {
        RoomOutDto room = roomBusiness.addSingleRoom(userId, addRoomInDto);
        return Created(nameof(addSingleRoom), room);
    }

    [HttpGet("AllGround")]
    public ActionResult<List<RoomPositionOutDto>> getAllPositions()
    {
        return roomBusiness.getAllPositions();
    }

    [HttpPut("unmanage")]
    public IActionResult unManageRoom(UnManageInDto unManageInDto)
    {
        int res = roomBusiness.unManageRoom(unManageInDto);
        if (res == -1)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("getRoom/{roomId}")]
    public ActionResult<RoomOutDto> getSingleRoom(string roomId)
    {
        return roomBusiness.getSingleRoom(roomId);
    }

    [HttpGet("favorite/{userId}")]
    public ActionResult<List<RoomOutDto>> getRoomWithFavorite(string userId)
    {
        return roomBusiness.getRoomWithFavorite(userId);
    }

    [HttpGet("favorite/{userId}/{roomId}")]
    public ActionResult<RoomOutDto> getSingleRoomWithFavorite(string userId, string roomId)
    {
        return roomBusiness.getSingleRoomWithFavorite(userId, roomId);
    }

    [HttpPost("free")]
    public ActionResult<FreeRoomOutDto> getFreeRooms([FromBody] FreeRoomInFto freeRoomInFto)
    {
        var rooms = roomBusiness.getFreeRooms(freeRoomInFto.StartTime, freeRoomInFto.EndTime);
        var freeRoomGetAllDto = roomBusiness.generateRoomGetAllDto(rooms);
        return freeRoomGetAllDto;
    }
}