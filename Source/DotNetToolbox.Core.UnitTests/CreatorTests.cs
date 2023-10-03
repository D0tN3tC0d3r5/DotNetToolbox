namespace System;

public class CreatorTests {
    // ReSharper disable once ClassNeverInstantiated.Local
    private class TestClass {
    }

    [Fact]
    public void Instance_NoArgs_CreatesObjectOfType() {
        // Arrange & Act
        var instance = Creator.CreateInstance<TestClass>();

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
    public void Instance_WithArgs_CreatesObjectOfTypeAndSetsValues() {
        // Arrange
        const string expectedValue = "Test";

        // Act
        var instance = Creator.CreateInstance<TestClassWithArgs>(expectedValue);

        // Assert
        instance.Should().NotBeNull();
        instance.Should().BeOfType<TestClassWithArgs>();
        instance.Value.Should().Be(expectedValue);
    }
}
