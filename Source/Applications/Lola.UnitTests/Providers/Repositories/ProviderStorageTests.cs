namespace Lola.Providers.Repositories;

public class ProviderStorageTests {
    [Fact]
    public void Constructor_ShouldInitializeCorrectly() {
        // Arrange
        var mockConfiguration = Substitute.For<IConfiguration>();

        // Act
        var subject = new ProviderStorage(mockConfiguration);

        // Assert
        subject.Should().BeAssignableTo<IProviderStorage>();
        subject.Should().BeAssignableTo<JsonFilePerTypeStorage<ProviderEntity, uint>>();
    }

    [Fact]
    public void Add_ShouldAssignIncrementalKeys() {
        // Arrange
        var mockConfiguration = Substitute.For<IConfiguration>();
        var providerHandler = Substitute.For<IProviderHandler>();
        var context = new Map { ["ProviderHandler"] = providerHandler };
        var subject = new ProviderStorage(mockConfiguration);
        var entity1 = new ProviderEntity { Name = "Alpha" };
        var entity2 = new ProviderEntity { Name = "Bravo" };

        // Act
        subject.Add(entity1, context);
        subject.Add(entity2, context);

        // Assert
        entity1.Key.Should().Be(1u);
        entity2.Key.Should().Be(2u);
    }

    [Fact]
    public void GetAll_ShouldReturnEntitiesWithCorrectKeys() {
        // Arrange
        var mockConfiguration = Substitute.For<IConfiguration>();
        var providerHandler = Substitute.For<IProviderHandler>();
        var context = new Map { ["ProviderHandler"] = providerHandler };
        var subject = new ProviderStorage(mockConfiguration);
        var entity1 = new ProviderEntity { Name = "Alpha" };
        var entity2 = new ProviderEntity { Name = "Bravo" };

        // Act
        subject.Add(entity1, context);
        subject.Add(entity2, context);
        var allEntities = subject.GetAll().ToList();

        // Assert
        allEntities.Should().HaveCount(2);
        allEntities[0].Key.Should().Be(1u);
        allEntities[1].Key.Should().Be(2u);
    }
}
