namespace Lola.Providers.Repositories;

public class ProviderEntityTests {
    private readonly IProviderHandler _mockProviderHandler;
    private readonly IMap _mockContext;

    public ProviderEntityTests() {
        _mockProviderHandler = Substitute.For<IProviderHandler>();
        _mockContext = Substitute.For<IMap>();
        _mockContext.GetRequiredValueAs<IProviderHandler>(nameof(ProviderHandler)).Returns(_mockProviderHandler);
    }

    [Fact]
    public void Validate_WithValidEntity_ShouldReturnSuccess() {
        // Arrange
        var entity = new ProviderEntity {
            Key = 1,
            Name = "Test Provider",
            ApiKey = "valid-api-key",
        };

        _mockProviderHandler.GetByName(entity.Name).Returns((ProviderEntity?)null);

        // Act
        var result = entity.Validate(_mockContext);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(null, "The name is required.")]
    [InlineData("", "The name is required.")]
    [InlineData(" ", "The name is required.")]
    public void ValidateName_WithInvalidName_ShouldReturnError(string? name, string expectedError) {
        // Act
        var result = ProviderEntity.ValidateName(name, _mockProviderHandler);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == expectedError);
    }

    [Fact]
    public void ValidateName_WithExistingName_ShouldReturnError() {
        // Arrange
        const string name = "Existing Provider";
        _mockProviderHandler.GetByName(name).Returns(new ProviderEntity());

        // Act
        var result = ProviderEntity.ValidateName(name, _mockProviderHandler);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "A provider with this name is already registered.");
    }

    [Theory]
    [InlineData(null, "The API Key is required.")]
    [InlineData("", "The API Key is required.")]
    [InlineData(" ", "The API Key is required.")]
    public void ValidateApiKey_WithInvalidApiKey_ShouldReturnError(string? apiKey, string expectedError) {
        // Act
        var result = ProviderEntity.ValidateApiKey(apiKey);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == expectedError);
    }

    [Fact]
    public void ValidateApiKey_WithValidApiKey_ShouldReturnSuccess() {
        // Arrange
        const string apiKey = "valid-api-key";

        // Act
        var result = ProviderEntity.ValidateApiKey(apiKey);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
