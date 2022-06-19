using CMS.Models;
namespace CMS.Business;

public interface IPersonalInfoBusiness
{
    public PersonalInfoOutDto getPersonalInformation(string userId);

    public Task<int> modifyPersonalInfo(string userId, PersonalInfoOutDto personalInfo);

    public Task<int> modifyAvatar(ModifyAvatarInDto modifyAvatarInDto);

    public string? getAvatarUrl(string userId);

    public List<PersonalInfoOutDto> getAllUsers();

    public Task<int> addUser(PersonalInfoOutDto personalInfo);

    public void grant(GrantInDto grantInDto);
}