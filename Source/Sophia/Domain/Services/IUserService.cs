
namespace Sophia.WebApp.Services;

public interface IUserService {
    Task<UserProfileData> GetCurrentUserProfile();
}
