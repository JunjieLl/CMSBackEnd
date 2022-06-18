using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CMS.Business;
using CMS.Models;

namespace CMS.Controllers;


[Route("api")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly cmsContext context;

    private readonly IMapper iMapper;

    private readonly ILoginBusiness iLoginBusiness;

    public LoginController(cmsContext context, IMapper iMapper, ILoginBusiness iLoginBusiness)
    {
        this.context = context;
        this.iMapper = iMapper;
        this.iLoginBusiness = iLoginBusiness;
    }

    [HttpPost("login")]
    public ActionResult<User> login(LoginInDto loginModel)
    {
        var user = iLoginBusiness.Login(loginModel);
        if (null == user)
        {
            return NotFound();
        }

        return Ok(iMapper.Map<LoginOutDto>(user));
    }

    [HttpGet("isLogin")]
    public ActionResult<IsLoginOutDto> isLogin()
    {
        return Ok(new IsLoginOutDto { userId = "1953474", identity = "学生" });
    }
}