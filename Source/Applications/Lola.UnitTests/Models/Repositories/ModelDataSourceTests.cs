namespace Lola.Models.Repositories;

public class ModelDataSourceTests {
    [Fact]
    public void Constructor_ShouldInitializeCorrectly() {
        // Arrange
        var mockStorage = Substitute.For<IModelStorage>();
        var mockProviders = new Lazy<IProviderDataSource>(() => Substitute.For<IProviderDataSource>());

        // Act
        var subject = new ModelDataSource(mockStorage, mockProviders);

        // Assert
        subject.Should().BeAssignableTo<IModelDataSource>();
        subject.Should().BeAssignableTo<DataSource<IModelStorage, ModelEntity, string>>();
    }

    [Fact]
    public void GetAll_WithoutIncludingProviders_ShouldReturnModelsWithoutProviders() {
        // Arrange
        var mockStorage = Substitute.For<IModelStorage>();
        var mockProviders = new Lazy<IProviderDataSource>(() => Substitute.For<IProviderDataSource>());
        var subject = new ModelDataSource(mockStorage, mockProviders);

        var models = new[]
        {
            new ModelEntity { Key = "model1", ProviderKey = 1 },
            new ModelEntity { Key = "model2", ProviderKey = 2 },
        };

        mockStorage.GetAll(Arg.Any<Expression<Func<ModelEntity, bool>>?>()).Returns(models);

        // Act
        var result = subject.GetAll(includeProviders: false);

        // Assert
        result.Should().BeEquivalentTo(models);
        result.All(m => m.Provider == null).Should().BeTrue();
    }

    [Fact]
    public void GetAll_WithIncludingProviders_ShouldReturnModelsWithProviders() {
        // Arrange
        var mockStorage = Substitute.For<IModelStorage>();
        var mockProviderDataSource = Substitute.For<IProviderDataSource>();
        var mockProviders = new Lazy<IProviderDataSource>(() => mockProviderDataSource);
        var subject = new ModelDataSource(mockStorage, mockProviders);

        var models = new[]
        {
            new ModelEntity { Key = "model1", ProviderKey = 1 },
            new ModelEntity { Key = "model2", ProviderKey = 2 },
        };

        var providers = new[]
        {
            new ProviderEntity { Key = 1, Name = "Provider1" },
            new ProviderEntity { Key = 2, Name = "Provider2" },
        };

        mockStorage.GetAll(Arg.Any<Expression<Func<ModelEntity, bool>>?>()).Returns(models);
        mockProviderDataSource.GetAll().Returns(providers);

        // Act
        var result = subject.GetAll(includeProviders: true);

        // Assert
        result.Should().HaveCount(2);
        result[0].Provider.Should().BeEquivalentTo(providers[0]);
        result[1].Provider.Should().BeEquivalentTo(providers[1]);
    }

    [Fact]
    public void GetSelected_ShouldReturnSelectedModelWithProvider() {
        // Arrange
        var mockStorage = Substitute.For<IModelStorage>();
        var mockProviderDataSource = Substitute.For<IProviderDataSource>();
        var mockProviders = new Lazy<IProviderDataSource>(() => mockProviderDataSource);
        var subject = new ModelDataSource(mockStorage, mockProviders);

        var selectedModel = new ModelEntity { Key = "selected", ProviderKey = 1, Selected = true };
        var provider = new ProviderEntity { Key = 1, Name = "Provider1" };

        mockStorage.Find(Arg.Any<Expression<Func<ModelEntity, bool>>>()).Returns(selectedModel);
        mockProviderDataSource.FindByKey(1u).Returns(provider);

        // Act
        var result = subject.GetSelected();

        // Assert
        result.Should().BeEquivalentTo(selectedModel);
        result.Provider.Should().BeEquivalentTo(provider);
    }

    [Fact]
    public void FindByKey_WithIncludeProvider_ShouldReturnModelWithProvider() {
        // Arrange
        var mockStorage = Substitute.For<IModelStorage>();
        var mockProviderDataSource = Substitute.For<IProviderDataSource>();
        var mockProviders = new Lazy<IProviderDataSource>(() => mockProviderDataSource);
        var subject = new ModelDataSource(mockStorage, mockProviders);

        var model = new ModelEntity { Key = "model1", ProviderKey = 1 };
        var provider = new ProviderEntity { Key = 1, Name = "Provider1" };

        mockStorage.FindByKey("model1").Returns(model);
        mockProviderDataSource.FindByKey(1u).Returns(provider);

        // Act
        var result = subject.FindByKey("model1", includeProvider: true);

        // Assert
        result.Should().BeEquivalentTo(model);
        result.Provider.Should().BeEquivalentTo(provider);
    }

    [Fact]
    public void FindByKey_WithoutIncludeProvider_ShouldReturnModelWithoutProvider() {
        // Arrange
        var mockStorage = Substitute.For<IModelStorage>();
        var mockProviders = new Lazy<IProviderDataSource>(() => Substitute.For<IProviderDataSource>());
        var subject = new ModelDataSource(mockStorage, mockProviders);

        var model = new ModelEntity { Key = "model1", ProviderKey = 1 };

        mockStorage.FindByKey("model1").Returns(model);

        // Act
        var result = subject.FindByKey("model1", includeProvider: false);

        // Assert
        result.Should().BeEquivalentTo(model);
        result.Provider.Should().BeNull();
    }
}
