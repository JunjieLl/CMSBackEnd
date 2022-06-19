using CMS.Models;

namespace CMS.Business;

public interface IActivityBusiness
{
    public List<ActivitiesGetDto> getAllActivities();

    public List<ActivitiesGetDto> filterManagerActivities(List<ActivitiesGetDto> activitiesGetDtos, string roomManagerId);
}