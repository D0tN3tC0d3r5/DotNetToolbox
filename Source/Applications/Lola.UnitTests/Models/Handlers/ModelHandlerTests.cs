namespace Lola.Models.Handlers;

public class ModelHandlerTests {
    private readonly IModelDataSource _mockDataSource;
    private readonly TrackedLogger<ModelHandler> _mockLogger;
    private readonly ModelHandler _handler;
    private readonly List<ModelEntity> _records = [];

    public ModelHandlerTests() {
        var mockApplication = Substitute.For<IApplication>();
        _mockDataSource = Substitute.For<IModelDataSource>();
        _mockDataSource.Records.Returns(_records);
        _mockDataSource.Expression.Returns(_records.AsQueryable().Expression);
        _mockDataSource.ElementType.Returns(_records.AsQueryable().ElementType);
        var mockProviderHandler = Substitute.For<IProviderHandler>();
        var mockLazyProviderHandler = new Lazy<IProviderHandler>(mockProviderHandler);
        _mockLogger = new();
        _handler = new(mockApplication, _mockDataSource, mockLazyProviderHandler, _mockLogger);
    }

    [Fact]
    public void Selected_WhenNoSelectedModel_ShouldReturnNull() {
        // Arrange
        _mockDataSource.GetSelected().Returns((ModelEntity?)null);

        // Act
        var result = _handler.Selected;

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Selected_WhenSelectedModelExists_ShouldReturnModel() {
        // Arrange
        var expectedModel = new ModelEntity { Key = "model1", Name = "Test Model" };
        _mockDataSource.GetSelected().Returns(expectedModel);

        // Act
        var result = _handler.Selected;

        // Assert
        result.Should().BeEquivalentTo(expectedModel);
    }

    [Fact]
    public void List_ShouldReturnOrderedModels() {
        // Arrange
        var models = new[]
        {
            new ModelEntity { Id = 2, Key = "model2", Name = "B Model" },
            new ModelEntity { Id = 1, Key = "model1", Name = "A Model" },
        };
        _mockDataSource.GetAll(Arg.Any<Expression<Func<ModelEntity, bool>>>()).Returns(models);

        // Act
        var result = _handler.List();

        // Assert
        result.Should().BeInAscendingOrder(m => m.Name);
    }

    [Fact]
    public void GetByKey_ShouldReturnCorrectModel() {
        // Arrange
        var expectedModel = new ModelEntity { Id = 1, Key = "model1", Name = "Test Model" };
        _mockDataSource.FindById(1).Returns(expectedModel);

        // Act
        var result = _handler.GetById(1);

        // Assert
        result.Should().BeEquivalentTo(expectedModel);
    }

    [Fact]
    public void Find_ShouldReturnCorrectModel() {
        // Arrange
        var expectedModel = new ModelEntity { Id = 1, Key = "model1", Name = "Test Model" };
        _mockDataSource.Find(Arg.Any<Expression<Func<ModelEntity, bool>>>()).Returns(expectedModel);

        // Act
        var result = _handler.Find(m => m.Name == "Test Model");

        // Assert
        result.Should().BeEquivalentTo(expectedModel);
    }

    [Fact]
    public void Add_WhenModelAlreadyExists_ShouldThrowException() {
        // Arrange
        var model = new ModelEntity { Id = 1, Key = "model1", Name = "Test Model" };
        _mockDataSource.FindById(1).Returns(model);
        _mockDataSource.Add(Arg.Any<ModelEntity>(), Arg.Any<IMap?>()).Returns(Result.Invalid("A model with the key 'model1' already exists."));

        // Act & Assert
        _handler.Invoking(h => h.Add(model))
            .Should().Throw<ValidationException>()
            .WithMessage("Validation failed.");
    }

    [Fact]
    public void Add_WhenModelIsNew_ShouldAddModelAndSetSelected() {
        // Arrange
        var model = new ModelEntity { Id = 1, Key = "model1", Name = "Test Model" };
        _mockDataSource.FindById(1).Returns((ModelEntity?)null);
        _mockDataSource.Add(Arg.Any<ModelEntity>(), Arg.Any<IMap?>()).Returns(Result.Success());

        // Act
        _handler.Add(model);

        // Assert
        _mockDataSource.Received(1).Add(model, Arg.Any<IMap?>());
        model.Selected.Should().BeTrue();
        _mockLogger.Logs.Should().ContainSingle(log => log.Level == LogLevel.Information
                                                    && log.Message.Contains(model.Key)
                                                    && log.Message.Contains(model.Name));
    }

    [Fact]
    public void Update_WhenModelDoesNotExist_ShouldThrowException() {
        // Arrange
        var model = new ModelEntity { Id = 1, Key = "model1", Name = "Test Model" };
        _mockDataSource.FindById(1).Returns((ModelEntity?)null);

        // Act & Assert
        _handler.Invoking(h => h.Update(model))
            .Should().Throw<InvalidOperationException>()
            .WithMessage("Model with id '1' not found.");
    }

    [Fact]
    public void Update_WhenModelExists_ShouldUpdateModel() {
        // Arrange
        var model = new ModelEntity { Id = 1, Key = "model1", Name = "Test Model" };
        _mockDataSource.FindById(1).Returns(model);
        _mockDataSource.Update(Arg.Any<ModelEntity>(), Arg.Any<IMap?>()).Returns(Result.Success());

        // Act
        _handler.Update(model);

        // Assert
        _mockDataSource.Received(1).Update(model, Arg.Any<IMap?>());
        _mockLogger.Logs.Should().ContainSingle(log => log.Level == LogLevel.Information
                                                    && log.Message.Contains(model.Key)
                                                    && log.Message.Contains(model.Name));
    }

    [Fact]
    public void Remove_WhenModelDoesNotExist_ShouldThrowException() {
        // Arrange
        _mockDataSource.FindById(1, false).Returns((ModelEntity?)null);

        // Act & Assert
        _handler.Invoking(h => h.Remove(1))
            .Should().Throw<InvalidOperationException>()
            .WithMessage("Model with id '1' not found.");
    }

    [Fact]
    public void Remove_WhenModelExists_ShouldRemoveModel() {
        // Arrange
        var model = new ModelEntity { Id = 1, Key = "model1", Name = "Test Model" };
        _mockDataSource.FindById(1, false).Returns(model);

        // Act
        _handler.Remove(1);

        // Assert
        _mockDataSource.Received(1).Remove(1);
        _mockLogger.Logs.Should().ContainSingle(log => log.Level == LogLevel.Information
                                                    && log.Message.Contains(model.Key)
                                                    && log.Message.Contains(model.Name));
    }

    [Fact]
    public void ListByProvider_ShouldReturnCorrectModels() {
        // Arrange
        var models = new[]
        {
            new ModelEntity { Key = "model1", ProviderId = 1 },
            new ModelEntity { Key = "model2", ProviderId = 1 },
        };
        _mockDataSource.GetAll(Arg.Any<Expression<Func<ModelEntity, bool>>>()).Returns(models);

        // Act
        var result = _handler.ListByProvider(1);

        // Assert
        result.Should().BeEquivalentTo(models);
    }

    [Fact]
    public void Select_WhenModelDoesNotExist_ShouldThrowException() {
        // Arrange
        _mockDataSource.FindById(1).Returns((ModelEntity?)null);

        // Act & Assert
        _handler.Invoking(h => h.Select(1))
            .Should().Throw<InvalidOperationException>()
            .WithMessage("Model '1' not found.");
    }

    [Fact]
    public void Select_WhenModelExists_ShouldSetInternalAndLogInfo() {
        // Arrange
        var model = new ModelEntity { Id = 1, Key = "model1", Name = "Test Model" };
        _records.Add(model);
        _mockDataSource.FindById(1).Returns(model);
        _mockDataSource.GetSelected().Returns(model);

        // Act
        _handler.Select(1);

        // Assert
        _handler.Selected.Should().BeEquivalentTo(model);
        _mockLogger.Logs.Should().ContainSingle(log => log.Level == LogLevel.Information
                                                    && log.Message.Contains(model.Key)
                                                    && log.Message.Contains(model.Name));
    }
}
