namespace AI.Sample.Users.Repositories;

public class UserRepositoryStrategy(IConfigurationRoot configuration)
    : JsonFileRepositoryStrategy<IUserRepository, UserEntity, string>("users", configuration),
      IUserRepositoryStrategy {
    protected override string FirstKey { get; } = "1";

    protected override bool TryGenerateNextKey(out string next) {
        next = LastUsedKey == default ? FirstKey : $"{uint.Parse(LastUsedKey) + 1}";
        LastUsedKey = next;
        return true;
    }
}
