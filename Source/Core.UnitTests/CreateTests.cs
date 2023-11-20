namespace System;

public class CreateTests {
    // ReSharper disable once ClassNeverInstantiated.Local
    private class TestClass {
    }

    [Fact]
    public void Create_NoArgs_CreatesObjectOfTypeT() {
        // Arrange & Act
        var instance = Create.Instance<TestClass>();

        // Assert
        instance.Should().NotBeNull();
        instance.Should().BeOfType<TestClass>();
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private class TestClassWithArgs {
        public string Value { get; }

        public TestClassWithArgs(string value) {
            Value = value;
        }
    }

    [Fact]
    public void Create_WithArgs_CreatesObjectOfTypeTAndSetsValues() {
        // Arrange
        const string expectedValue = "Test";

        // Act
        var instance = Create.Instance<TestClassWithArgs>(expectedValue);

        // Assert
        instance.Should().NotBeNull();
        instance.Should().BeOfType<TestClassWithArgs>();
        instance.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void Create_WithWrongArgs_Throws() {
        // Act
        var action = () => Create.Instance<TestClassWithArgs>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Create_WithProvider_CreatesObjectOfTypeT() {
        // Arrange
        var provider = Substitute.For<IServiceProvider>();

        // Act
        var instance = Create.Instance<TestClass>(provider);

        // Assert
        instance.Should().NotBeNull();
        instance.Should().BeOfType<TestClass>();
    }

    [Fact]
    public void Create_WithProvider_WithWrongArgs_CreatesObjectOfTypeT() {
        // Arrange
        var provider = Substitute.For<IServiceProvider>();

        // Act
        var action = () => Create.Instance<TestClassWithArgs>(provider);

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }
}
