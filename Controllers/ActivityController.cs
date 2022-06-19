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
}