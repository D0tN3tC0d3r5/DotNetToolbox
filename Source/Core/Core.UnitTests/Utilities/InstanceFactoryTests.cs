namespace DotNetToolbox.Utilities;

public class InstanceFactoryTests {
    // ReSharper disable once ClassNeverInstantiated.Local
    private class TestClass;

    [Fact]
    public void Create_NoArgs_CreatesObjectOfTypeT() {
        // Arrange & Act
        var instance = InstanceFactory.Create<TestClass>();

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
        var instance = InstanceFactory.Create<TestClassWithArgs>(expectedValue);

        // Assert
        instance.Should().NotBeNull();
        var subject = instance.Should().BeOfType<TestClassWithArgs>().Subject;
        subject.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void Create_WithWrongArgs_Throws() {
        // Act
        var action = () => InstanceFactory.Create<TestClassWithArgs>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Create_WithProvider_CreatesObjectOfTypeT() {
        // Arrange
        var provider = For<IServiceProvider>();

        // Act
        var instance = InstanceFactory.Create<TestClass>(provider);

        // Assert
        instance.Should().NotBeNull();
        instance.Should().BeOfType<TestClass>();
    }

    [Fact]
    public void Create_WithProvider_WithWrongArgs_CreatesObjectOfTypeT() {
        // Arrange
        var provider = For<IServiceProvider>();

        // Act
        var action = () => InstanceFactory.Create<TestClassWithArgs>(provider);

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }
}
