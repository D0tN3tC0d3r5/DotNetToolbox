using AIUserProfile = DotNetToolbox.AI.Jobs.UserProfile;

namespace Lola.UserProfile.Repositories;

public class UserProfileEntity
    : Entity<UserProfileEntity, uint> {
    public bool Internal { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Language { get; set; } = "English";
    public List<string> Facts { get; } = [];

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        result += ValidateName(Name);
        return result;
    }

    public static implicit operator AIUserProfile(UserProfileEntity entity)
        => new(entity.Key) {
            Name = entity.Name,
            Language = entity.Language,
            Facts = entity.Facts,
        };

    public static Result ValidateName(string? name) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(name))
            result += new ValidationError("Soory yout name cannot be empty or only white spaces.", nameof(Name));
        return result;
    }
}
