using Microsoft.AspNetCore.Mvc;
using CMS.Models;
using CMS.Business;

namespace CMS.Controllers;

[Route("api/Activity")]
[ApiController]
public class ActivityController : ControllerBase
{
    private readonly IActivityBusiness activityBusiness;

    public ActivityController(IActivityBusiness activityBusiness)
    {
        this.activityBusiness = activityBusiness;
    }

    [HttpGet]
    public ActionResult<Dictionary<string, List<ActivitiesGetDto>>> getActivity(
        [FromQuery] string? commonUserId, [FromQuery] string? roomManagerId, [FromQuery] bool? lastMonth
    )
    {
        List<ActivitiesGetDto> activitiesGetDtos = activityBusiness.getAllActivities();
        if (commonUserId != null)
        {
            activitiesGetDtos.RemoveAll(a => !commonUserId.Equals(a.CommonUserId));
        }

        if (lastMonth != null)
        {
            TimeSpan timeSpan = TimeSpan.FromDays(30);
            DateTime curTime = DateTime.Now;
            activitiesGetDtos.RemoveAll(a => curTime - a.StartTime > timeSpan);
        }

        if (roomManagerId != null)
        {
            activitiesGetDtos = activityBusiness.filterManagerActivities(activitiesGetDtos, roomManagerId);
        }

        return activitiesGetDtos.GroupBy(a => a.ActivityStatus).ToDictionary(a => a.First().ActivityStatus, a => a.ToList());
    }

    [HttpGet("room/{roomId}")]
    public ActionResult<List<RoomActivityGetDto>> getRoomActivity(string roomId)
    {
        return activityBusiness.getRoomActivity(roomId);
    }

    [HttpPut]
    public IActionResult modifyActivity(ActivityPutDto activityPutDto)
    {
        int res = activityBusiness.modifyActivity(activityPutDto);
        if (-1 == res)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPost]
    public IActionResult addActivity(ActivityPostDto activityPostDto)
    {
        var a = activityBusiness.addActivity(activityPostDto);
        return Created(nameof(addActivity), a);
    }

    [HttpGet("occupy")]
    public ActionResult<List<OccupyWechatDto>> getOccupyTime(string date, string roomId)
    {
        return activityBusiness.getOccupyTime(date, roomId);
    }

    [HttpPost("Period")]
    public IActionResult addPeriod(PeriodActivityPostDto periodActivityPostDto)
    {
        activityBusiness.addPeriod(periodActivityPostDto);
        return Ok();
    }

    [HttpPut("Comment")]
    public IActionResult addComment(ActivityCommentPutDto activityCommentPutDto)
    {
        int res = activityBusiness.addComment(activityCommentPutDto);
        if (-1 == res)
        {
            return NotFound();
        }
        return NoContent();
    }
}