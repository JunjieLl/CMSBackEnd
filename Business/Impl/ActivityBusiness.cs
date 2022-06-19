using CMS.Models;
using AutoMapper;
namespace CMS.Business;

public class ActivityBusiness : IActivityBusiness
{
    private readonly cmsContext context;

    private readonly IMapper mapper;

    private readonly IRoomBusiness roomBusiness;

    public ActivityBusiness(cmsContext context, IMapper mapper, IRoomBusiness roomBusiness)
    {
        this.roomBusiness = roomBusiness;
        this.mapper = mapper;
        this.context = context;
    }

    public List<ActivitiesGetDto> getAllActivities()
    {
        var activities = context.Activities.ToList();

        List<ActivitiesGetDto> activitiesGetDtos = new List<ActivitiesGetDto>();

        activities.ForEach(a =>
        {
            ActivitiesGetDto activitiesGetDto = mapper.Map<ActivitiesGetDto>(a);
            activitiesGetDto.UserName = context.Users.Single(u => u.UserId.Equals(a.CommonUserId)).UserName;
            var roomInfo = context.Rooms
            .Where(r => r.RoomId.Equals(a.RoomId))
            .Select(r =>
                new
                {
                    r.Image,
                    r.Building,
                    r.Floor,
                    r.RoomName
                }
            )
            .Single();
            activitiesGetDto.Image = roomInfo.Image;
            activitiesGetDto.Building = roomInfo.Building;
            activitiesGetDto.Floor = roomInfo.Floor;
            activitiesGetDto.RoomName = roomInfo.RoomName;

            if (a.ActivityStatus.Equals("待举办"))
            {
                DateTime curDate = DateTime.Now;
                if (curDate > a.StartTime)
                {
                    activitiesGetDto.ActivityStatus = "待反馈";
                }
            }
            else if (a.ActivityStatus.Equals("被驳回"))
            {
                ModifyRecord? modifyRecord = context.ModifyRecords
                .Join(context.RoomManagers, m => m.UserId, rm => rm.UserId, (m, _) => m)
                .SingleOrDefault(m => m.ActivityId.Equals(a.ActivityId));
                if (null != modifyRecord)
                {
                    activitiesGetDto.Reason = modifyRecord.Reason;
                }
            }
            else
            {
                ModifyRecord? modifyRecord = context.ModifyRecords.SingleOrDefault(m => m.ActivityId.Equals(a.ActivityId));
                if (null != modifyRecord)
                {
                    activitiesGetDto.Reason = modifyRecord.Reason;
                }
            }
            activitiesGetDtos.Add(activitiesGetDto);
        });
        return activitiesGetDtos;
    }

    public List<ActivitiesGetDto> filterManagerActivities(List<ActivitiesGetDto> activitiesGetDtos, string roomManagerId)
    {
        var managerRooms = context.Manages.Where(m => m.UserId.Equals(roomManagerId)).Select(r => r.RoomId).ToHashSet();

        activitiesGetDtos.RemoveAll(
            a => !managerRooms.Contains(a.RoomId)
        );
        return activitiesGetDtos;
    }
}