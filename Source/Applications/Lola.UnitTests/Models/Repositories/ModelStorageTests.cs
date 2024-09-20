namespace Lola.Models.Repositories;

public class ModelStorageTests {
    [Fact]
    public void Constructor_ShouldInitializeCorrectly() {
        // Arrange
        var mockConfiguration = Substitute.For<IConfiguration>();

        // Act
        var subject = new ModelStorage(mockConfiguration);

        // Assert
        subject.Should().BeAssignableTo<IModelStorage>();
        subject.Should().BeAssignableTo<JsonFilePerTypeStorage<ModelEntity, uint>>();
    }

    [Fact]
    public void Add_ShouldAssignIncrementalKeys() {
        // Arrange
        var mockConfiguration = Substitute.For<IConfiguration>();
        var modelHandler = Substitute.For<IModelHandler>();
        var providerHandler = Substitute.For<IProviderHandler>();
        providerHandler.GetById(Arg.Any<uint>()).Returns(new ProviderEntity());
        var context = new Map {
            [nameof(EntityAction)] = EntityAction.Insert,
            [nameof(ModelHandler)] = modelHandler,
            [nameof(ProviderHandler)] = providerHandler,
        };
        var subject = new ModelStorage(mockConfiguration);
        var entity1 = new ModelEntity { Id = 1, Key = "alpha", Name = "Alpha", ProviderId = 1 };
        var entity2 = new ModelEntity { Id = 2, Key = "bravo", Name = "Bravo", ProviderId = 1 };

        // Act
        subject.Add(entity1, context);
        subject.Add(entity2, context);

        // Assert
        entity1.Key.Should().Be("alpha");
        entity2.Key.Should().Be("bravo");
    }

    [Fact]
    public void GetAll_ShouldReturnEntitiesWithCorrectKeys() {
        // Arrange
        var mockConfiguration = Substitute.For<IConfiguration>();
        var modelHandler = Substitute.For<IModelHandler>();
        var providerHandler = Substitute.For<IProviderHandler>();
        providerHandler.GetById(Arg.Any<uint>()).Returns(new ProviderEntity());
        var context = new Map {
            [nameof(EntityAction)] = EntityAction.Insert,
            [nameof(ModelHandler)] = modelHandler,
            [nameof(ProviderHandler)] = providerHandler,
        };
        var subject = new ModelStorage(mockConfiguration);
        var entity1 = new ModelEntity { Id = 1, Key = "alpha", Name = "Alpha", ProviderId = 1 };
        var entity2 = new ModelEntity { Id = 2, Key = "bravo", Name = "Bravo", ProviderId = 1 };

        // Act
        subject.Add(entity1, context);
        subject.Add(entity2, context);
        var allEntities = subject.GetAll().ToList();

        // Assert
        allEntities.Should().HaveCount(2);
        allEntities[0].Key.Should().Be("alpha");
        allEntities[1].Key.Should().Be("bravo");
    }
}
