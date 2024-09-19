using Model = DotNetToolbox.AI.Models.Model;

namespace Lola.Models.Repositories;

public class ModelEntityTests {
    private readonly IModelHandler _mockModelHandler;
    private readonly IProviderHandler _mockProviderHandler;
    private readonly IMap _mockContext;

    public ModelEntityTests() {
        _mockModelHandler = Substitute.For<IModelHandler>();
        _mockProviderHandler = Substitute.For<IProviderHandler>();
        _mockContext = Substitute.For<IMap>();
        _mockContext.GetRequiredValueAs<IModelHandler>(nameof(ModelHandler)).Returns(_mockModelHandler);
        _mockContext.GetRequiredValueAs<IProviderHandler>(nameof(ProviderHandler)).Returns(_mockProviderHandler);
    }

    [Fact]
    public void Validate_WithValidEntity_ShouldReturnSuccess() {
        // Arrange
        var entity = new ModelEntity {
            Key = "model1",
            Name = "Test Model",
            ProviderId = 1,
            InputCostPerMillionTokens = 0.1m,
            OutputCostPerMillionTokens = 0.2m,
            TrainingDateCutOff = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
        };

        _mockModelHandler.GetById(entity.Id).Returns((ModelEntity?)null);
        _mockModelHandler.GetByName(entity.Name).Returns((ModelEntity?)null);
        _mockProviderHandler.GetById(entity.ProviderId).Returns(new ProviderEntity());

        // Act
        var result = entity.Validate(_mockContext);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(null, "The identifier is required.")]
    [InlineData("", "The identifier is required.")]
    [InlineData(" ", "The identifier is required.")]
    public void ValidateKey_WithInvalidKey_ShouldReturnError(string? key, string expectedError) {
        // Act
        var result = ModelEntity.ValidateKey(key, _mockModelHandler);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == expectedError);
    }

    [Fact]
    public void ValidateKey_WithExistingKey_ShouldReturnError() {
        // Arrange
        const string key = "existingKey";
        _mockModelHandler.GetById(Arg.Any<uint>()).Returns(new ModelEntity { Key = key });

        // Act
        var result = ModelEntity.ValidateKey(key, _mockModelHandler);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "A model with this identifier is already registered.");
    }

    [Theory]
    [InlineData(null, "The name is required.")]
    [InlineData("", "The name is required.")]
    [InlineData(" ", "The name is required.")]
    public void ValidateName_WithInvalidName_ShouldReturnError(string? name, string expectedError) {
        // Act
        var result = ModelEntity.ValidateName(name, _mockModelHandler);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == expectedError);
    }

    [Fact]
    public void ValidateName_WithExistingName_ShouldReturnError() {
        // Arrange
        const string name = "Existing Name";
        _mockModelHandler.GetByName(name).Returns(new ModelEntity());

        // Act
        var result = ModelEntity.ValidateName(name, _mockModelHandler);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "A model with this name is already registered.");
    }

    [Fact]
    public void ValidateProvider_WithNonExistentProvider_ShouldReturnError() {
        // Arrange
        const uint providerKey = 999;
        _mockProviderHandler.GetById(providerKey).Returns((ProviderEntity)null!);

        // Act
        var result = ModelEntity.ValidateProvider(providerKey, _mockProviderHandler);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "The provider does not exist.");
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(-1)]
    public void ValidateInputCost_WithNegativeValue_ShouldReturnError(decimal cost) {
        // Act
        var result = ModelEntity.ValidateInputCost(cost);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "The input cost per million tokens must be greater than or equal to zero.");
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(-1)]
    public void ValidateOutputCost_WithNegativeValue_ShouldReturnError(decimal cost) {
        // Act
        var result = ModelEntity.ValidateOutputCost(cost);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "The input cost per million tokens must be greater than or equal to zero.");
    }

    [Fact]
    public void ValidateDateCutOff_WithFutureDate_ShouldReturnError() {
        // Arrange
        var futureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        // Act
        var result = ModelEntity.ValidateDateCutOff(futureDate);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "The training data cut off date must be in the past.");
    }

    [Fact]
    public void ValidateDateCutOff_WithDateBeforeJan2021_ShouldReturnError() {
        // Arrange
        var oldDate = DateOnly.Parse("2020-12-31");

        // Act
        var result = ModelEntity.ValidateDateCutOff(oldDate);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "The training data cut off date must be after Jan. 2021.");
    }

    [Fact]
    public void ImplicitConversion_ToModel_ShouldConvertCorrectly() {
        // Arrange
        var entity = new ModelEntity {
            Id = 1,
            Key = "model1",
            Name = "Test Model",
            ProviderId = 1,
            Provider = new() { Name = "Test Provider" },
            MaximumContextSize = 4096,
            MaximumOutputTokens = 1000,
            TrainingDateCutOff = DateOnly.Parse("2023-01-01"),
        };

        // Act
        Model model = entity;

        // Assert
        model.Id.Should().Be(entity.Id);
        model.Key.Should().Be(entity.Key);
        model.Provider.Should().Be(entity.Provider.Name);
        model.Name.Should().Be(entity.Name);
        model.MaximumContextSize.Should().Be(entity.MaximumContextSize);
        model.MaximumOutputTokens.Should().Be(entity.MaximumOutputTokens);
        model.TrainingDataCutOff.Should().Be(entity.TrainingDateCutOff);
    }
}
