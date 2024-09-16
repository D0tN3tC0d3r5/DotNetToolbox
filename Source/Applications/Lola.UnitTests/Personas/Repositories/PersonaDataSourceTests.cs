namespace Lola.Personas.Repositories;

public class PersonaDataSourceTests {
    [Fact]
    public void Constructor_ShouldInitializeCorrectly() {
        // Arrange
        var mockStorage = Substitute.For<IPersonaStorage>();

        // Act
        var subject = new PersonaDataSource(mockStorage);

        // Assert
        subject.Should().BeAssignableTo<IPersonaDataSource>();
        subject.Should().BeAssignableTo<DataSource<IPersonaStorage, PersonaEntity, uint>>();
    }
}
