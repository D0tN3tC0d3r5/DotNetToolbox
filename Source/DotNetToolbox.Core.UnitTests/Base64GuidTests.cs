namespace System;

public class Base64GuidTests {
    [Fact]
    public void Parse_ReturnsBase64Guid() {
        // Arrange
        var input = Guid.NewGuid();
        var guidString = input.ToString();
        var base64GuidString = (string)new Base64Guid(input);

        // Act
        var subject1 = Base64Guid.Parse(guidString);
        var subject2 = Base64Guid.Parse(base64GuidString);

        // Assert
        subject1.Value.Should().Be(input);
        subject2.Value.Should().Be(input);
    }

    [Fact]
    public void TryParse_ReturnsTrueIfValid() {
        // Arrange
        var input = Guid.NewGuid();

        // Act
        var valid = Base64Guid.TryParse(input.ToString(), out _);
        var invalid = Base64Guid.TryParse("Invalid", out _);

        // Assert
        valid.Should().BeTrue();
        invalid.Should().BeFalse();
    }

    [Fact]
    public void ToString_ReturnsBase64Guid() {
        // Arrange
        var input = Guid.NewGuid();
        var subject = new Base64Guid {
            Value = input,
        };
        // Act
        string text = subject;

        // Assert
        text.Should().NotBeNullOrWhiteSpace();
        text.Should().NotContain("/");
        text.Should().NotContain("+");
        text.Should().NotContain("=");
        text.Should().HaveLength(22);
    }

    [Fact]
    public void ToGuid_ReturnsGuid() {
        // Arrange
        var input = Guid.NewGuid();
        var subject = new Base64Guid(input);

        // Act
        Guid guid = subject;

        // Assert
        guid.Should().Be(input);
    }

    [Fact]
    public void FromBase64_SetProperly() {
        // Arrange
        var input = new string('A', 22);

        // Act
        Base64Guid subject = input;

        // Assert
        subject.Value.Should().Be(Guid.Empty);
    }

    [Fact]
    public void FromGuidEmpty_ToString_SetProperly() {
        // Act
        string subject = (Base64Guid)Guid.Empty;

        // Assert
        subject.Should().Be(string.Empty);
    }

    [Fact]
    public void FromNullBase64_SetProperly() {
        // Act
        Base64Guid subject = default(string)!;

        // Assert
        subject.Value.Should().Be(Guid.Empty);
    }

    [Fact]
    public void FromInvalid_Throws() {
        // Arrange
        var input = "invalid";

        // Act
        var action = () => {
            Base64Guid _ = input;
        };

        // Assert
        action.Should().Throw<FormatException>();
    }

    [Fact]
    public void FromGuid_SetProperly() {
        // Arrange
        var input = Guid.NewGuid();

        // Act
        Base64Guid subject = input;

        // Assert
        subject.Value.Should().Be(input);
    }

    [Fact]
    public void FromGuidEmpty_ToBase64Guid_SetProperly() {
        // Arrange
        var input = Guid.Empty;

        // Act
        Base64Guid subject = input;

        // Assert
        subject.Value.Should().Be(input);
    }
}
