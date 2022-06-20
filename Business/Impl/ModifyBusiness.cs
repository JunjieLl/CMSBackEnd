
using CMS.Models;
using AutoMapper;

namespace CMS.Business;


public class ModifyBusiness : IModifyBusiness
{
    private readonly cmsContext context;

    private readonly IMapper mapper;

    public ModifyBusiness(cmsContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public int cancelActivity(ModifyRecordDto modifyRecordDto)
    {
        Activity? activity = context.Activities.SingleOrDefault(a => a.ActivityId.Equals(modifyRecordDto.ActivityId));
        if (null == activity)
        {
            return -1;
        }
        activity.ActivityStatus = "已取消";

        modifyRecordDto.ModifyTime = DateTime.Now;
        ModifyRecord? modifyRecord = context.ModifyRecords
        .SingleOrDefault(m => m.ActivityId.Equals(modifyRecordDto.ActivityId));
        if (null == modifyRecord)
        {
            modifyRecordDto.RecordId = (int.Parse(context.ModifyRecords.Max(m => m.RecordId)) + 1).ToString();
            context.ModifyRecords.Add(mapper.Map<ModifyRecord>(modifyRecordDto));
        }
        else
        {
            modifyRecord.Reason = modifyRecordDto.Reason;
            modifyRecord.ModifyTime = modifyRecordDto.ModifyTime;
            modifyRecord.UserId = modifyRecordDto.UserId;
        }

        context.SaveChanges();
        return 1;
    }

    public int rejectActivity(ModifyRecordDto modifyRecordDto)
    {
        Activity? activity = context.Activities.SingleOrDefault(a => a.ActivityId.Equals(modifyRecordDto.ActivityId));
        if (null == activity)
        {
            return -1;
        }
        activity.ActivityStatus = "被驳回";

        modifyRecordDto.ModifyTime = DateTime.Now;
        ModifyRecord? modifyRecord = context.ModifyRecords
        .SingleOrDefault(m => m.ActivityId.Equals(modifyRecordDto.ActivityId));
        if (null == modifyRecord)
        {
            modifyRecordDto.RecordId = (int.Parse(context.ModifyRecords.Max(m => m.RecordId)) + 1).ToString();
            context.ModifyRecords.Add(mapper.Map<ModifyRecord>(modifyRecordDto));
        }
        else
        {
            modifyRecord.Reason = modifyRecordDto.Reason;
            modifyRecord.ModifyTime = modifyRecordDto.ModifyTime;
            modifyRecord.UserId = modifyRecordDto.UserId;
        }

        context.SaveChanges();
        return 1;
    }

}