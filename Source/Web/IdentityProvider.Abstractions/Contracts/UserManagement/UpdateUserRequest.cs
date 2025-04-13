namespace WebApi.Contracts.UserManagement;

/// <summary>
/// Represents the data allowed for updating an existing user's profile information.
/// </summary>
public record UpdateUserRequest
    : UserRequest {
    // Add here the properties that should be updated.
}
