namespace AI.Sample.UserProfile.Repositories;

public class UserProfileRepository(IUserProfileRepositoryStrategy strategy)
    : Repository<IUserProfileRepositoryStrategy, UserProfileEntity, uint>(strategy),
      IUserProfileRepository;
