namespace Lola.UserProfile.Repositories;

public class UserProfileStorage(IConfiguration configuration)
    : JsonFilePerTypeStorage<UserProfileEntity, uint>("users", configuration),
      IUserProfileStorage {
    protected override uint FirstKey { get; } = 1;

    protected override bool TryGenerateNextKey(out uint next) {
        next = LastUsedKey == default ? FirstKey : LastUsedKey + 1;
        LastUsedKey = next;
        return true;
    }
}
