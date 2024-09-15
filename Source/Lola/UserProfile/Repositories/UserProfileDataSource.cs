using DotNetToolbox.Data.DataSources;

namespace Lola.UserProfile.Repositories;

public class UserProfileDataSource(IUserProfileStorage strategy)
    : DataSource<IUserProfileStorage, UserProfileEntity, uint>(strategy),
      IUserProfileDataSource;
