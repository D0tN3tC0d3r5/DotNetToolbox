namespace Lola.UserProfile.Repositories;

public class UserProfileStorageTests {
    [Fact]
    public void Constructor_ShouldInitializeCorrectly() {
        // Arrange
        var mockConfiguration = Substitute.For<IConfiguration>();

        // Act
        var subject = new UserProfileStorage(mockConfiguration);

        // Assert
        subject.Should().BeAssignableTo<IUserProfileStorage>();
        subject.Should().BeAssignableTo<JsonFilePerTypeStorage<UserProfileEntity, uint>>();
    }

    [Fact]
    public void Add_ShouldAssignIncrementalIds() {
        // Arrange
        var mockConfiguration = Substitute.For<IConfiguration>();
        var userprofileHandler = Substitute.For<IUserProfileHandler>();
        var context = new Map { ["UserProfileHandler"] = userprofileHandler };
        var subject = new UserProfileStorage(mockConfiguration);
        var entity1 = new UserProfileEntity { Name = "Alpha" };
        var entity2 = new UserProfileEntity { Name = "Bravo" };

        // Act
        subject.Add(entity1, context);
        subject.Add(entity2, context);

        // Assert
        entity1.Id.Should().Be(1u);
        entity2.Id.Should().Be(2u);
    }

    [Fact]
    public void GetAll_ShouldReturnEntitiesWithCorrectIds() {
        // Arrange
        var mockConfiguration = Substitute.For<IConfiguration>();
        var userprofileHandler = Substitute.For<IUserProfileHandler>();
        var context = new Map { ["UserProfileHandler"] = userprofileHandler };
        var subject = new UserProfileStorage(mockConfiguration);
        var entity1 = new UserProfileEntity { Name = "Alpha" };
        var entity2 = new UserProfileEntity { Name = "Bravo" };

        // Act
        subject.Add(entity1, context);
        subject.Add(entity2, context);
        var allEntities = subject.GetAll().ToList();

        // Assert
        allEntities.Should().HaveCount(2);
        allEntities[0].Id.Should().Be(1u);
        allEntities[1].Id.Should().Be(2u);
    }
}
