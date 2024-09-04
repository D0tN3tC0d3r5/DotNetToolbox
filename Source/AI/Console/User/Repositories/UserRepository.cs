namespace AI.Sample.Users.Repositories;

public class UserRepository(IUserRepositoryStrategy strategy)
    : Repository<IUserRepositoryStrategy, UserEntity, string>(strategy),
      IUserRepository;
