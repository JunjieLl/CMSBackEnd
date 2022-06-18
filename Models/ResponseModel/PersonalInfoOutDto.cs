namespace CMS.Models;

public class PersonalInfoOutDto
{
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string Identity { get; set; } = null!;

    public string? ActivityStatus { get; set; }

    public int TimeLimit { get; set; }

    public int CountLimit { get; set; }

    public sbyte isSuperManager { get; set; }
}