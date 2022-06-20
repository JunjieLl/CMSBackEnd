using CMS.Models;

namespace CMS.Business;

public interface IModifyBusiness
{
    public int cancelActivity(ModifyRecordDto modifyRecordDto);

    public int rejectActivity(ModifyRecordDto modifyRecordDto);
}