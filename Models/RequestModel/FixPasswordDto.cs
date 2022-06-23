namespace CMS.Models;

public class FixPasswordDto
{
    public string password { get; set; }
    public string userId { get; set; }
    public string verificationCode { get; set; }
}