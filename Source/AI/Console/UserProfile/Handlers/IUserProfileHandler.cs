namespace AI.Sample.UserProfile.Handlers;

public interface IUserProfileHandler {
    UserProfileEntity Create(Action<UserProfileEntity>? setUp = null);
    UserProfileEntity? Get();
    void Set(UserProfileEntity user);
}
