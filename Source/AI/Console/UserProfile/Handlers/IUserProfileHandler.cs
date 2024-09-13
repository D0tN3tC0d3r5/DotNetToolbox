namespace AI.Sample.UserProfile.Handlers;

public interface IUserProfileHandler {
    UserProfileEntity Create(Action<UserProfileEntity>? setUp = null);
    UserProfileEntity? CurrentUser { get; }
    void Set(UserProfileEntity user);
}
