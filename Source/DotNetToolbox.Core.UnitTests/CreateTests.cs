namespace System;

public class CreateTests {
    // ReSharper disable once ClassNeverInstantiated.Local
    private class TestClass;

    [Fact]
    public void Create_NoArgs_CreatesObjectOfTypeT() {
        // Arrange & Act
        var instance = Create.Instance<TestClass>();

        // Assert
        _ = instance.Should().NotBeNull();
        _ = instance.Should().BeOfType<TestClass>();
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
        var instance = Create.Instance<TestClassWithArgs>(expectedValue);

        // Assert
        _ = instance.Should().NotBeNull();
        _ = instance.Should().BeOfType<TestClassWithArgs>();
        _ = instance.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void Create_WithWrongArgs_Throws() {
        // Act
        var action = () => Create.Instance<TestClassWithArgs>();

        // Assert
        _ = action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Create_WithProvider_CreatesObjectOfTypeT() {
        // Arrange
        var provider = Substitute.For<IServiceProvider>();

        // Act
        var instance = Create.Instance<TestClass>(provider);

        // Assert
        _ = instance.Should().NotBeNull();
        _ = instance.Should().BeOfType<TestClass>();
    }

    [Fact]
    public void Create_WithProvider_WithWrongArgs_CreatesObjectOfTypeT() {
        // Arrange
        var provider = Substitute.For<IServiceProvider>();

        // Act
        var action = () => Create.Instance<TestClassWithArgs>(provider);

        // Assert
        _ = action.Should().Throw<InvalidOperationException>();
    }
}
