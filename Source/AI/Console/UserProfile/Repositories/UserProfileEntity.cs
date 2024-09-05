using AIUserProfile = DotNetToolbox.AI.Jobs.UserProfile;

namespace AI.Sample.UserProfile.Repositories;

public class UserProfileEntity
    : Entity<UserProfileEntity, uint> {
    public bool Internal { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Language { get; set; } = "English";
    public List<string> Facts { get; } = [];

    public override Result Validate(IContext? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Name)) result += new ValidationError("Name is required.", nameof(Name));
        return result;
    }

    public static implicit operator AIUserProfile(UserProfileEntity entity)
        => new(entity.Key) {
            Name = entity.Name,
            Language = entity.Language,
            Facts = entity.Facts,
        };
}
