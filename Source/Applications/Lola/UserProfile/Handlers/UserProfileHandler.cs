namespace Lola.UserProfile.Handlers;

public class UserProfileHandler(IUserProfileDataSource dataSource, ILogger<UserProfileHandler> logger)
    : IUserProfileHandler {
    private readonly IUserProfileDataSource _dataSource = dataSource;
    private readonly ILogger<UserProfileHandler> _logger = logger;
    private UserProfileEntity? _currentUser;

    public UserProfileEntity Create(Action<UserProfileEntity>? setUp = null)
        => _dataSource.Create(setUp);
    public UserProfileEntity? CurrentUser
        => _currentUser ??= _dataSource.FirstOrDefault(i => !i.Internal);
    public void Set(UserProfileEntity user) {
        if (_dataSource.Any()) _dataSource.Update(user);
        else _dataSource.Add(user);
        _logger.LogInformation("User profile set.");
    }
}
