namespace Lola.Providers.Repositories;

public class ProviderDataSourceTests {
    [Fact]
    public void Constructor_ShouldInitializeCorrectly() {
        // Arrange
        var mockStorage = Substitute.For<IProviderStorage>();

        // Act
        var subject = new ProviderDataSource(mockStorage);

        // Assert
        subject.Should().BeAssignableTo<IProviderDataSource>();
        subject.Should().BeAssignableTo<DataSource<IProviderStorage, ProviderEntity, uint>>();
    }
}
