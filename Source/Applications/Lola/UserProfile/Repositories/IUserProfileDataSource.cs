using DotNetToolbox.Data.DataSources;

namespace Lola.UserProfile.Repositories;

public interface IUserProfileDataSource
    : IDataSource<UserProfileEntity, uint>;
