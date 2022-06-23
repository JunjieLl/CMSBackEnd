using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CMS.Business;
using CMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;


namespace CMS.Controllers;


[Route("api")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly cmsContext context;

    private readonly IMapper iMapper;

    private readonly ILoginBusiness loginBusiness;


    public LoginController(cmsContext context, IMapper iMapper, ILoginBusiness iLoginBusiness)
    {
        this.context = context;
        this.iMapper = iMapper;
        this.loginBusiness = iLoginBusiness;
    }

    [HttpPost("login")]
    public ActionResult<User> login(LoginInDto loginModel)
    {
        var user = loginBusiness.Login(loginModel);
        if (null == user)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [Authorize]
    [HttpGet("isLogin")]
    public async Task<ActionResult<IsLoginOutDto>> isLogin()
    {
        //access token
        var authenticationInfo = await HttpContext.GetTokenAsync("access_token");
        //get info
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(authenticationInfo);
        var payload = jwtSecurityToken.Payload;
        var claims = payload.Claims.ToDictionary(c => c.Type, c => c.Value);

        return Ok(new IsLoginOutDto { userId = claims.GetValueOrDefault("userId", ""), identity = claims.GetValueOrDefault("identity", "") });
    }
}