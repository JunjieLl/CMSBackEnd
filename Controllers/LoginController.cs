using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace CMS.Controllers;

[Route("api/")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly cmsContext context;

    private readonly IMapper iMapper;

    public LoginController(cmsContext context, IMapper iMapper)
    {
        this.context = context;
        this.iMapper = iMapper;
    }

    [HttpPost("login")]
    public ActionResult<User> login(LoginModel loginModel)
    {
        Console.WriteLine(loginModel.password, loginModel.userId);
        var user = context.Users.Single(user => user.UserId.Equals(loginModel.userId));

        // var user = await context.Users.FindAsync(loginModel.userId);
        if (null == user)
        {
            return NotFound();
        }

        return Ok(iMapper.Map<LoginDto>(user));
    }

    [HttpGet("isLogin")]
    public ActionResult<IsLoginDto> isLogin()
    {
        return Ok(new IsLoginDto { userId = "1953474", identity = "学生" });
    }
}