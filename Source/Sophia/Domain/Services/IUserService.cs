
namespace Sophia.Services;

public interface IUserService {
    Task<UserData> GetCurrentUserProfile();
}
