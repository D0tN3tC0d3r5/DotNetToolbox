namespace DotNetToolbox;

public class CreateInstanceTests {
    // ReSharper disable once ClassNeverInstantiated.Local
    private class TestClass;

    [Fact]
    public void Create_NoArgs_CreatesObjectOfTypeT() {
        // Arrange & Act
        var instance = CreateInstance.Of<TestClass>();

        // Assert
        instance.Should().NotBeNull();
        instance.Should().BeOfType<TestClass>();
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private class TestClassWithArgs(string value) {
        public string Value { get; } = value;
    }

    [Fact]
    public void Create_WithArgs_CreatesObjectOfTypeTAndSetsValues() {
        // Arrange
        const string expectedValue = "Test";

        // Act
        var instance = CreateInstance.Of<TestClassWithArgs>(expectedValue);

        // Assert
        instance.Should().NotBeNull();
        var subject = instance.Should().BeOfType<TestClassWithArgs>().Subject;
        subject.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void Create_WithWrongArgs_Throws() {
        // Act
        var action = () => CreateInstance.Of<TestClassWithArgs>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Create_WithProvider_CreatesObjectOfTypeT() {
        // Arrange
        var provider = For<IServiceProvider>();

        // Act
        var instance = CreateInstance.Of<TestClass>(provider);

        // Assert
        instance.Should().NotBeNull();
        instance.Should().BeOfType<TestClass>();
    }

    [Fact]
    public void Create_WithProvider_WithWrongArgs_CreatesObjectOfTypeT() {
        // Arrange
        var provider = For<IServiceProvider>();

        // Act
        var action = () => CreateInstance.Of<TestClassWithArgs>(provider);

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }
}
