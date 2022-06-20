using CMS.Models;

namespace CMS.Business;

public interface IActivityBusiness
{
    public List<ActivitiesGetDto> getAllActivities();

    public List<ActivitiesGetDto> filterManagerActivities(List<ActivitiesGetDto> activitiesGetDtos, string roomManagerId);

    public List<RoomActivityGetDto> getRoomActivity(string roomId);

    public int modifyActivity(ActivityPutDto activityPutDto);

    public Activity addActivity(ActivityPostDto activityPostDto);

    public List<OccupyWechatDto> getOccupyTime(string date, string roomId);

    public void addPeriod(PeriodActivityPostDto periodActivityPostDto);

    public int addComment(ActivityCommentPutDto activityCommentPutDto);
}