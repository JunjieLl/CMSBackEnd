using CMS.Models;

namespace CMS.Business;

public class LoginBusiness : ILoginBusiness
{
    private readonly cmsContext context;

    public LoginBusiness(cmsContext context)
    {
        this.context = context;
    }

    public User? Login(LoginInDto loginModel)
    {
        var user = context.Users.Single(u => u.UserId.Equals(loginModel.userId)
        && u.Password!.Equals(loginModel.password));
        return user;
    }
}