using CMS.Models;


namespace CMS.Business;

public interface ILoginBusiness
{
    public LoginOutDto? Login(LoginInDto loginModel);
}