namespace AI.Sample.Users.Repositories;

public class UserRepositoryStrategy(IConfigurationRoot configuration)
    : JsonFileRepositoryStrategy<IUserRepository, UserEntity, uint>("users", configuration),
      IUserRepositoryStrategy {
    protected override uint FirstKey { get; } = 1;

    protected override bool TryGenerateNextKey(out uint next) {
        next = LastUsedKey == default ? FirstKey : LastUsedKey + 1;
        LastUsedKey = next;
        return true;
    }
}
