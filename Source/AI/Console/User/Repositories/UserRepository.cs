namespace AI.Sample.Users.Repositories;

public class UserRepository(IUserRepositoryStrategy strategy)
    : Repository<IUserRepositoryStrategy, UserEntity, uint>(strategy),
      IUserRepository;
