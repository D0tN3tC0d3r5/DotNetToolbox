namespace Sophia.WebApp.Components.Account;

public class ApplicationUser : IdentityUser {
    public ApplicationUserProfile Profile { get; set; } = new();
}

