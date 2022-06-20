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

    public List<RoomActivityGetDto> getRoomActivity(string roomId)
    {
        var a = DateTime.Now + TimeSpan.FromMinutes(23) + TimeSpan.FromHours(8);
        var b = a.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
        var roomActivityGetDtos = context.Activities.Where(a => a.RoomId.Equals(roomId)
        && (a.ActivityStatus.Equals("待举办") || a.ActivityStatus.Equals("待反馈")
        || a.ActivityStatus.Equals("已举办") || a.ActivityStatus.Equals("已反馈")))
        .Select(a => new RoomActivityGetDto
        {
            Id = a.ActivityId,
            Title = a.ActivityName,
            Start = (a.StartTime.AddHours(8)).ToString("yyyy'-'MM'-'dd'T'HH':'mm"),
            End = ((a.StartTime.AddHours(8).AddMinutes(a.Duration))).ToString("yyyy'-'MM'-'dd'T'HH':'mm")
        }).ToList();
        return roomActivityGetDtos;
    }

    public int modifyActivity(ActivityPutDto activityPutDto)
    {
        Activity? activity = context.Activities.SingleOrDefault(a => a.ActivityId.Equals(activityPutDto.ActivityId));
        if (activity == null)
        {
            return -1;
        }
        activity.ActivityName = activityPutDto.ActivityName;
        activity.ActivityStatus = activityPutDto.ActivityStatus;
        activity.StartTime = activityPutDto.StartTime;
        activity.Duration = activityPutDto.Duration;
        activity.ActivityDescription = activityPutDto.ActivityDescription;
        activity.PoliticallyRelevant = activityPutDto.PoliticallyRelevant;
        activity.RoomId = activityPutDto.RoomId;
        //add record
        ModifyRecordDto modifyRecordDto = new ModifyRecordDto();
        modifyRecordDto.ActivityId = activityPutDto.ActivityId;
        modifyRecordDto.Reason = activityPutDto.Reason;
        modifyRecordDto.UserId = activityPutDto.CommonUserId;
        modifyRecordDto.ModifyTime = DateTime.Now;
        ModifyRecord? modifyRecord = context.ModifyRecords.
        SingleOrDefault(m => m.ActivityId.Equals(activityPutDto.ActivityId));
        if (null != modifyRecord)
        {
            modifyRecord.Reason = modifyRecordDto.Reason;
            modifyRecord.ModifyTime = modifyRecordDto.ModifyTime;
            modifyRecord.UserId = modifyRecordDto.UserId;
        }
        else
        {
            int id = int.Parse(context.ModifyRecords.Max(m => m.RecordId)) + 1;
            string recordId = id.ToString();
            modifyRecordDto.RecordId = recordId;
            context.ModifyRecords.Add(mapper.Map<ModifyRecord>(modifyRecordDto));
        }

        //update activity

        context.SaveChanges();
        return 1;
    }

    public Activity addActivity(ActivityPostDto activityPostDto)
    {
        Activity activity = new Activity();
        activity.ActivityName = activityPostDto.ActivityName;
        activity.ActivityStatus = activityPostDto.ActivityStatus;
        activity.StartTime = activityPostDto.StartTime;
        activity.Duration = activityPostDto.Duration;
        activity.ActivityDescription = activityPostDto.ActivityDescription;
        activity.PoliticallyRelevant = activityPostDto.PoliticallyRelevant;
        activity.RoomId = activityPostDto.RoomId;
        activity.CommonUserId = activityPostDto.CommonUserId;
        activity.ActivityId = (int.Parse(context.Activities.Max(a => a.ActivityId)) + 1).ToString();
        context.Activities.Add(activity);
        context.SaveChanges();
        return activity;
    }

    public List<OccupyWechatDto> getOccupyTime(string date, string roomId)
    {
        return context.Activities
        .Where(a => a.RoomId.Equals(roomId))
        .ToList()
         .Where(a => a.StartTime.ToString("yyyy-MM-dd").Equals(date))
        .Select(a => new OccupyWechatDto
        {
            StartTime = a.StartTime.ToString("HH:mm"),
            Duration = a.Duration
        }).ToList();
    }

    public void addPeriod(PeriodActivityPostDto periodActivityPostDto)
    {
        ActivityPostDto activityPostDto = mapper.Map<ActivityPostDto>(periodActivityPostDto);

        for (int i = 0; i < periodActivityPostDto.managerWeek; ++i)
        {
            activityPostDto.StartTime = periodActivityPostDto.StartTime.AddDays(7 * i);
            addActivity(activityPostDto);
        }


    }

    public int addComment(ActivityCommentPutDto activityCommentPutDto)
    {
        Activity? activity = context.Activities.SingleOrDefault(a => a.ActivityId.Equals(activityCommentPutDto.ActivityId));
        if (null == activity)
        {
            return -1;
        }

        activity.EvaluateTime = DateTime.Now;
        activity.Content = activityCommentPutDto.Content;
        activity.ManagerUserId = activityCommentPutDto.ManagerUserId;
        activity.ActivityStatus = activityCommentPutDto.ActivityStatus;

        context.SaveChanges();
        return 1;
    }
}