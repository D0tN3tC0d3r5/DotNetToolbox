// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Text;

public class UrlSafeBase64StringTests {
    private const string _stringValue = "1234567890123456";
    private const string _base64Value = "MTIzNDU2Nzg5MDEyMzQ1Ng";
    private static readonly byte[] _bytesValue = "1234567890123456"u8.ToArray();

    [Fact]
    public void DefaultConstructor_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String();

        // Assert
        result.Bytes.Should().BeEmpty();
        result.PlainText.Should().Be(string.Empty);
        result.Encoded.Should().Be(string.Empty);
    }

    [Fact]
    public void ConstructorWithNullBytes_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String(default(byte[]));

        // Assert
        result.Bytes.Should().BeEmpty();
        result.PlainText.Should().Be(string.Empty);
        result.Encoded.Should().Be(string.Empty);
    }

    [Fact]
    public void ConstructorWithNullString_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String(default(string));

        // Assert
        result.Bytes.Should().BeEmpty();
        result.PlainText.Should().Be(string.Empty);
        result.Encoded.Should().Be(string.Empty);
    }

    [Fact]
    public void StringConstructor_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String(_stringValue);

        // Assert
        result.Bytes.Should().BeEquivalentTo(_bytesValue);
        result.PlainText.Should().Be(_stringValue);
        result.Encoded.Should().Be(_base64Value);
    }

    [Fact]
    public void Base64Constructor_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String(_base64Value);

        // Assert
        result.Bytes.Should().BeEquivalentTo(_bytesValue);
        result.PlainText.Should().Be(_stringValue);
        result.Encoded.Should().Be(_base64Value);
    }

    [Fact]
    public void BytesConstructor_ReturnsBase64Guid() {
        // Act
        var result = new UrlSafeBase64String(_bytesValue);

        // Assert
        result.Bytes.Should().BeEquivalentTo(_bytesValue);
        result.PlainText.Should().Be(_stringValue);
        result.Encoded.Should().Be(_base64Value);
    }

    [Fact]
    public void ImplicitConversionTo_ReturnCorrectValues() {
        // Act
        var subject = new UrlSafeBase64String(_bytesValue);

        // Act
        string text = subject;
        byte[] bytes = subject;

        // Assert
        text.Should().Be(_base64Value);
        bytes.Should().BeEquivalentTo(_bytesValue);
    }

    [Fact]
    public void ImplicitConversionFrom_ReturnCorrectValues() {
        // Act
        UrlSafeBase64String value0 = _stringValue;
        UrlSafeBase64String value1 = _base64Value;
        UrlSafeBase64String value3 = _bytesValue;

        // Assert
        value0.PlainText.Should().Be(_stringValue);
        value1.PlainText.Should().Be(_stringValue);
        value3.PlainText.Should().Be(_stringValue);
    }
}
