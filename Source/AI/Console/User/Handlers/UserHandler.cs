namespace AI.Sample.Users.Handlers;

public class UserHandler(IUserRepository repository, Lazy<IModelHandler> modelHandler, ILogger<UserHandler> logger)
    : IUserHandler {
    private readonly IUserRepository _repository = repository;
    private readonly Lazy<IModelHandler> _modelHandler = modelHandler;
    private readonly ILogger<UserHandler> _logger = logger;

    public UserEntity Create(Action<UserEntity>? setUp = null)
        => _repository.Create(setUp);
    public UserEntity? Get()
        => _repository.FirstOrDefault();
    public void Set(UserEntity user) {
        if (_repository.Any()) _repository.Update(user);
        else _repository.Add(user);
        _logger.LogInformation("User profile set.");
    }
}
