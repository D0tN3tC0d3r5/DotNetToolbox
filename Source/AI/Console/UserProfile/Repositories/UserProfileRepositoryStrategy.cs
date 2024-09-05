namespace AI.Sample.UserProfile.Repositories;

public class UserProfileRepositoryStrategy(IConfigurationRoot configuration)
    : JsonFilePerTypeRepositoryStrategy<IUserProfileRepository, UserProfileEntity, uint>("users", configuration),
      IUserProfileRepositoryStrategy {
    protected override uint FirstKey { get; } = 1;

    protected override bool TryGenerateNextKey(out uint next) {
        next = LastUsedKey == default ? FirstKey : LastUsedKey + 1;
        LastUsedKey = next;
        return true;
    }
}
