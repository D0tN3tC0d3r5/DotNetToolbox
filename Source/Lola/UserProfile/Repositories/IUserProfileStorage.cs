using DotNetToolbox.Data.Storages;

namespace Lola.UserProfile.Repositories;

public interface IUserProfileStorage
    : IStorage<UserProfileEntity, uint>;
