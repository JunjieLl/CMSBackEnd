

namespace CMS.Business;

public interface ILoginBusiness
{
    public User? Login(LoginInDto loginModel);
}