using DotNetToolbox.Data.DataSources;

namespace Lola.UserProfile.Repositories;

public class UserProfileDataSource(IUserProfileStorage storage)
    : DataSource<IUserProfileStorage, UserProfileEntity, uint>(storage),
      IUserProfileDataSource;
