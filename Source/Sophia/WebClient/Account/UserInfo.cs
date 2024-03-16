namespace Sophia.WebClient.Account;

public class UserInfo {
    public const string DefaultName = "User";
    public string Name { get; set; } = DefaultName;
    public required string UserId { get; set; }
    public required string Email { get; set; }
}
