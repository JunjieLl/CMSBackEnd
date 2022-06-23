
using Microsoft.AspNetCore.Mvc;
using CMS.Models;
using CMS.Business;

namespace CMS.Controllers;

[ApiController]
[Route("api/PersonalInformation")]
public class PersonalInformationController : ControllerBase
{

    private readonly IPersonalInfoBusiness personalInfoBusiness;


    public PersonalInformationController(IPersonalInfoBusiness personalInfoBusiness, IEmailBusiness emailBusiness)
    {
        this.personalInfoBusiness = personalInfoBusiness;
    }


    [HttpGet("{userId}")]
    public ActionResult<PersonalInfoOutDto> getPersonalInformation(string userId)
    {
        PersonalInfoOutDto personalInfoOutDto = personalInfoBusiness.getPersonalInformation(userId);
        if (null == personalInfoOutDto)
        {
            return NotFound();
        }
        return Ok(personalInfoOutDto);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> modifyPersonalInfo(string userId, [FromBody] PersonalInfoOutDto personalInfo)
    {
        int res = await personalInfoBusiness.modifyPersonalInfo(userId, personalInfo);
        if (res == -1)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpPost("Avatar")]
    public async Task<IActionResult> modifyAvatar(ModifyAvatarInDto modifyAvatarInDto)
    {
        int res = await personalInfoBusiness.modifyAvatar(modifyAvatarInDto);
        return NoContent();
    }

    [HttpGet("Avatar/{userId}")]
    public ActionResult<string?> getAvatarUrl(string userId)
    {
        return personalInfoBusiness.getAvatarUrl(userId);
    }

    [HttpGet("All")]
    public ActionResult<List<PersonalInfoOutDto>> getAllUsers()
    {
        return personalInfoBusiness.getAllUsers();
    }

    [HttpPost("Add")]
    public async Task<ActionResult<PersonalInfoOutDto>> addUser(PersonalInfoOutDto personalInfo)
    {
        if (personalInfo.ActivityStatus == null)
        {
            personalInfo.ActivityStatus = "1";
        }
        int res = await personalInfoBusiness.addUser(personalInfo);
        if (res == -1)
        {
            return BadRequest();

        }
        return Created(nameof(addUser), personalInfo);
    }

    [HttpPut("Grant")]
    public IActionResult grant(GrantInDto grantInDto)
    {
        personalInfoBusiness.grant(grantInDto);
        return NoContent();
    }

    [HttpGet("Email")]
    public ActionResult<string> sendEmail(string userId)
    {
        string? res = personalInfoBusiness.sendEmail(userId);
        if (res == null)
        {
            return NotFound();
        }
        return res;
    }

    [HttpPut("UserPassword")]
    public IActionResult modifyPassword(FixPasswordDto fixPasswordDto)
    {
        int res = personalInfoBusiness.modifyPassword(fixPasswordDto);
        if (res == -1)
        {
            return NotFound();
        }
        return NoContent();
    }
}