namespace Lola.UserProfile.Repositories;

public class UserProfileDataSourceTests {
    [Fact]
    public void Constructor_ShouldInitializeCorrectly() {
        // Arrange
        var mockStorage = Substitute.For<IUserProfileStorage>();

        // Act
        var subject = new UserProfileDataSource(mockStorage);

        // Assert
        subject.Should().BeAssignableTo<IUserProfileDataSource>();
        subject.Should().BeAssignableTo<DataSource<IUserProfileStorage, UserProfileEntity, uint>>();
    }
}
