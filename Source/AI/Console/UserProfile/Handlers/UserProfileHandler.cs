namespace AI.Sample.UserProfile.Handlers;

public class UserProfileHandler(IUserProfileRepository repository, ILogger<UserProfileHandler> logger)
    : IUserProfileHandler {
    private readonly IUserProfileRepository _repository = repository;
    private readonly ILogger<UserProfileHandler> _logger = logger;

    public UserProfileEntity Create(Action<UserProfileEntity>? setUp = null)
        => _repository.Create(setUp);
    public UserProfileEntity? Get()
        => _repository.FirstOrDefault(i => !i.Internal);
    public void Set(UserProfileEntity user) {
        if (_repository.Any()) _repository.Update(user);
        else _repository.Add(user);
        _logger.LogInformation("User profile set.");
    }
}
