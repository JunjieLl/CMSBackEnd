public class LoginDto
{

    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string Identity { get; set; } = null!;

    public string? ActivityStatus { get; set; }

    public string? Avatar { get; set; }

    public string? TokenName { get; set; }
    
    public string? TokenVal { get; set; }
}