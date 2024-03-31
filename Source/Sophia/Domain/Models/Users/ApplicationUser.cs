using Microsoft.AspNetCore.Identity;

namespace Sophia.Models.Users;

public class ApplicationUser : IdentityUser {
    public ApplicationUserProfile Profile { get; set; } = new();
}

