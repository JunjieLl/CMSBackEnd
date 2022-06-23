

using Microsoft.AspNetCore.Mvc;
using CMS.Models;
using CMS.Business;

namespace CMS.Controllers;

[Route("api/ModifyRecord")]
[ApiController]

public class ModifyController : ControllerBase
{
    private readonly IModifyBusiness modifyBusiness;

    public ModifyController(IModifyBusiness modifyBusiness)
    {
        this.modifyBusiness = modifyBusiness;
    }


    [HttpPost]
    public IActionResult cancelActivity(ModifyRecordDto modifyRecordDto)
    {
        int res = modifyBusiness.cancelActivity(modifyRecordDto);
        if (res == -1)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpPost("reject")]
    public IActionResult rejectActivity(ModifyRecordDto modifyRecordDto)
    {
        int res = modifyBusiness.rejectActivity(modifyRecordDto);
        if (res == -1)
        {
            return NotFound();
        }

        return Ok();
    }

}