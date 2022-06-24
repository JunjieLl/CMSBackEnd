using CMS.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

using AutoMapper;
using System.Runtime.InteropServices;


namespace CMS.Business;

public class LoginBusiness : ILoginBusiness
{
    private readonly cmsContext context;

    private readonly IConfiguration configuration;

    private readonly IMapper mapper;

    [DllImport("Win32Dll.dll")]
    private static extern bool IsStringEqual(string s1, string s2);

    public LoginBusiness(cmsContext context, IConfiguration configuration, IMapper mapper)
    {


        this.configuration = configuration;
        this.context = context;
        this.mapper = mapper;
    }

    public LoginOutDto? Login(LoginInDto loginModel)
    {
        //var user = context.Users.SingleOrDefault(u => u.UserId.Equals(loginModel.userId)
        // && u.Password!.Equals(loginModel.password));
        var user = context.Users.SingleOrDefault(u => u.UserId.Equals(loginModel.userId));

        if (user == null||!IsStringEqual(user.Password,loginModel.password))
        {
            return null;
        }

        var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        //new Claim(JwtRegisteredClaimNames.Exp,)
                        new Claim("userId", user.UserId.ToString()),
                        new Claim("identity", user.Identity),
                    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signIn);
        var toeknVal = new JwtSecurityTokenHandler().WriteToken(token);
        LoginOutDto loginOutDto = mapper.Map<LoginOutDto>(user);
        loginOutDto.TokenVal = toeknVal;
        loginOutDto.TokenName = "helloc";
        return loginOutDto;
    }
}