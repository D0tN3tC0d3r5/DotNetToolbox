namespace AI.Sample.Users.Handlers;

public interface IUserHandler {
    UserEntity Create(Action<UserEntity>? setUp = null);
    UserEntity? Get();
    void Set(UserEntity user);
}
