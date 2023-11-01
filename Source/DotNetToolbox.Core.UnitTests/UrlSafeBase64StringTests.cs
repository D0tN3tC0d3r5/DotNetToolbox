namespace System;

public class UrlSafeBase64StringTests {
    private static readonly Guid _guidValue = new("34333231-3635-3837-3930-313233343536");
    private const string _stringValue = "1234567890123456";
    private const string _base64Value = "MTIzNDU2Nzg5MDEyMzQ1Ng";
    private static readonly byte[] _bytesValue = "1234567890123456"u8.ToArray();

    [Fact]
    public void DefaultConstructor_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String();

        // Assert
        _ = result.Bytes.Should().BeEmpty();
        _ = result.Text.Should().Be(string.Empty);
        _ = result.Base64.Should().Be(string.Empty);
        _ = result.IsGuid.Should().BeTrue();
        _ = result.Guid.Should().Be(Guid.Empty);
    }

    [Fact]
    public void ConstructorWithNullBytes_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String(default(byte[]));

        // Assert
        _ = result.Bytes.Should().BeEmpty();
        _ = result.Text.Should().Be(string.Empty);
        _ = result.Base64.Should().Be(string.Empty);
        _ = result.IsGuid.Should().BeTrue();
        _ = result.Guid.Should().Be(Guid.Empty);
    }

    [Fact]
    public void ConstructorWithNullString_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String(default(string)!);

        // Assert
        _ = result.Bytes.Should().BeEmpty();
        _ = result.Text.Should().Be(string.Empty);
        _ = result.Base64.Should().Be(string.Empty);
        _ = result.IsGuid.Should().BeTrue();
        _ = result.Guid.Should().Be(Guid.Empty);
    }

    [Fact]
    public void GuidConstructor_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String(_guidValue);

        // Assert
        _ = result.Bytes.Should().BeEquivalentTo(_bytesValue);
        _ = result.Text.Should().Be(_stringValue);
        _ = result.Base64.Should().Be(_base64Value);
        _ = result.IsGuid.Should().BeTrue();
        _ = result.Guid.Should().Be(_guidValue);
    }

    [Fact]
    public void StringConstructor_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String(_stringValue);

        // Assert
        _ = result.Bytes.Should().BeEquivalentTo(_bytesValue);
        _ = result.Text.Should().Be(_stringValue);
        _ = result.Base64.Should().Be(_base64Value);
        _ = result.IsGuid.Should().BeTrue();
        _ = result.Guid.Should().Be(_guidValue);
    }

    [Fact]
    public void Base64Constructor_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String(_base64Value);

        // Assert
        _ = result.Bytes.Should().BeEquivalentTo(_bytesValue);
        _ = result.Text.Should().Be(_stringValue);
        _ = result.Base64.Should().Be(_base64Value);
        _ = result.IsGuid.Should().BeTrue();
        _ = result.Guid.Should().Be(_guidValue);
    }

    [Fact]
    public void BytesConstructor_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String(_bytesValue);

        // Assert
        _ = result.Bytes.Should().BeEquivalentTo(_bytesValue);
        _ = result.Text.Should().Be(_stringValue);
        _ = result.Base64.Should().Be(_base64Value);
        _ = result.IsGuid.Should().BeTrue();
        _ = result.Guid.Should().Be(_guidValue);
    }

    [Fact]
    public void StringConstructor_InvalidBase64_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String("Not base 64.");

        // Assert
        _ = result.IsGuid.Should().BeFalse();
        _ = result.Invoking(s => s.Guid).Should().Throw<FormatException>();
        _ = result.Text.Should().Be("Not base 64.");
        _ = result.Bytes.Should().BeEquivalentTo("Not base 64."u8.ToArray());
    }

    [Fact]
    public void BytesConstructor_InvalidGuid_ReturnsBase64Guid() {
        var input = new byte[] {
            0xD6, 0xA8, 0x6D, 0x19, 0xC5, 0xAA, 0x06, 0x4F,
            0xD6, 0xA8, 0x6D, 0x19, 0xC5, 0xAA, 0x06, 0x4F,
            0xD6, 0xA8, 0x6D, 0x19, 0xC5, 0xAA, 0x06, 0x4F,
        };

        // Act
        var result = new UrlSafeBase64String(input);

        // Assert
        _ = result.IsGuid.Should().BeFalse();
        _ = result.Invoking(s => s.Guid).Should().Throw<FormatException>();
        _ = result.Text.Should().Be("\u05a8m\u0019Ū\u0006O\u05a8m\u0019Ū\u0006O\u05a8m\u0019Ū\u0006O");
        _ = result.Base64.Should().Be("1qhtGcWqBk_WqG0ZxaoGT9aobRnFqgZP");
        _ = result.Bytes.Should().BeEquivalentTo(input);
    }

    [Fact]
    public void ImplicitConversionTo_ReturnCorrectValues() {
        // Act
        var subject = new UrlSafeBase64String(_bytesValue);

        // Act
        string text = subject;
        Guid guid = subject;
        byte[] bytes = subject;

        // Assert
        _ = subject.IsGuid.Should().BeTrue();
        _ = text.Should().Be(_base64Value);
        _ = guid.Should().Be(_guidValue);
        _ = bytes.Should().BeEquivalentTo(_bytesValue);
    }

    [Fact]
    public void ImplicitConversionFrom_ReturnCorrectValues() {
        // Act
        UrlSafeBase64String value0 = _stringValue;
        UrlSafeBase64String value1 = _base64Value;
        UrlSafeBase64String value2 = _guidValue;
        UrlSafeBase64String value3 = _bytesValue;

        // Assert
        _ = value0.Text.Should().Be(_stringValue);
        _ = value1.Text.Should().Be(_stringValue);
        _ = value2.Text.Should().Be(_stringValue);
        _ = value3.Text.Should().Be(_stringValue);
    }
}
