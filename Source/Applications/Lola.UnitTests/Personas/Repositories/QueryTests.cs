namespace Lola.Personas.Repositories;

public class QueryTests {
    [Fact]
    public void Constructor_ShouldInitializePropertiesWithEmptyStrings() {
        // Act
        var query = new Query();

        // Assert
        query.Question.Should().BeEmpty();
        query.Explanation.Should().BeEmpty();
        query.Answer.Should().BeEmpty();
    }

    [Fact]
    public void Question_ShouldSetAndGetCorrectly() {
        // Arrange
        var query = new Query();
        const string expectedQuestion = "What is the capital of France?";

        // Act
        query.Question = expectedQuestion;

        // Assert
        query.Question.Should().Be(expectedQuestion);
    }

    [Fact]
    public void Explanation_ShouldSetAndGetCorrectly() {
        // Arrange
        var query = new Query();
        const string expectedExplanation = "This question tests knowledge of European geography.";

        // Act
        query.Explanation = expectedExplanation;

        // Assert
        query.Explanation.Should().Be(expectedExplanation);
    }

    [Fact]
    public void Answer_ShouldSetAndGetCorrectly() {
        // Arrange
        var query = new Query();
        const string expectedAnswer = "The capital of France is Paris.";

        // Act
        query.Answer = expectedAnswer;

        // Assert
        query.Answer.Should().Be(expectedAnswer);
    }

    [Fact]
    public void Properties_ShouldAllowEmptyStrings() {
        // Arrange
        var query = new Query {
            Question = "Initial question",
            Explanation = "Initial explanation",
            Answer = "Initial answer",
        };

        // Act
        query.Question = "";
        query.Explanation = "";
        query.Answer = "";

        // Assert
        query.Question.Should().BeEmpty();
        query.Explanation.Should().BeEmpty();
        query.Answer.Should().BeEmpty();
    }

    [Fact]
    public void Properties_ShouldAllowNullValues() {
        // Arrange
        var query = new Query {
            // Act
            Question = null!,
            Explanation = null!,
            Answer = null!,
        };

        // Assert
        query.Question.Should().BeNull();
        query.Explanation.Should().BeNull();
        query.Answer.Should().BeNull();
    }
}
