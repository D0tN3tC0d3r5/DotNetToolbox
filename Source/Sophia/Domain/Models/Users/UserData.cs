using Microsoft.AspNetCore.Identity;

namespace Sophia.Models.Users;

public class UserData : IdentityUser {
    public UserProfileData Profile { get; set; } = new();

    public UserProfile ToModel() => new() {
        Id = Id,
        Name = Profile.Name,
        Language = Profile.Language,
        Facts = Profile.Facts,
    };
}

