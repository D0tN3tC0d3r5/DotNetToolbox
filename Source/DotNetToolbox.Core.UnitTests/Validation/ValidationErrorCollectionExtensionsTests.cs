namespace System.Validation;

public class ValidationErrorCollectionExtensionsTests {
    [Fact]
    public void Merge_ReturnsMergedList() {
        // Arrange
        var errors1 = new List<ValidationError> {
            new("Source 1", "Some message 1."),
            new("Source 1", "Some message 2."),
            new("Source 2", "Some message 1."),
        };
        var errors2 = new List<ValidationError> {
            new("Source 1", "Some message 1."),
            new("Source 2", "Some message 2."),
            new("Source 2", "Some message 3."),
        };

        // Act
        errors1.Merge(errors2);

        // Assert
        errors1.Should().BeEquivalentTo(new ValidationError[] {
            new("Source 1", "Some message 1."),
            new("Source 2", "Some message 1."),
            new("Source 1", "Some message 2."),
            new("Source 2", "Some message 2."),
            new("Source 2", "Some message 3."),
        });
    }

    [Fact]
    public void Merge_Error_ReturnsMergedList() {
        // Arrange
        var errors1 = new List<ValidationError> {
            new("Source 1", "Some message 1."),
            new("Source 1", "Some message 2."),
            new("Source 2", "Some message 1."),
        };
        var errors2 = new ValidationError("Source 2", "Some message 3.");

        // Act
        errors1.Merge(errors2);

        // Assert
        errors1.Should().BeEquivalentTo(new ValidationError[] {
            new("Source 1", "Some message 1."),
            new("Source 2", "Some message 1."),
            new("Source 1", "Some message 2."),
            new("Source 2", "Some message 3."),
        });
    }
}
