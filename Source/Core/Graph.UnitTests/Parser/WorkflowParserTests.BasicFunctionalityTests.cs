namespace DotNetToolbox.Graph.Parser;

public partial class WorkflowParserTests {
    public class BasicFunctionalityTests : WorkflowParserTests {
        [Fact]
        public void Parse_EmptyInput_ReturnsEmptyWorkflow() {
            // Arrange
            const string script = "";
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
        }

        [Fact]
        public void Parse_SingleAction_ReturnsWorkflowWithOneAction() {
            // Arrange
            const string script = """
                                  DoSomething
                                  """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var start = result.Value.Should().BeOfType<ActionNode>().Subject;
            start.Tag.Should().Be("1");
            start.Label.Should().Be("DoSomething");
            start.Next.Should().BeNull();
        }

        [Fact]
        public void Parse_SingleActionWithExit_ReturnsWorkflowWithOneAction() {
            // Arrange
            const string script = """
                                  DoSomething
                                  EXIT
                                  """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var start = result.Value.Should().BeOfType<ActionNode>().Subject;
            start.Tag.Should().Be("1");
            start.Label.Should().Be("DoSomething");
            var end = start.Next.Should().BeOfType<ExitNode>().Subject;
            end.Tag.Should().Be("2");
            end.Label.Should().Be("end");
            end.ExitCode.Should().Be(0);
        }

        [Fact]
        public void Parse_MultipleActions_ReturnsWorkflowWithMultipleActions() {
            // Arrange
            const string script = """
                                  Action1
                                  Action2
                                  Action3
                                  """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var start = result.Value.Should().BeOfType<ActionNode>().Subject;
            start.Tag.Should().Be("1");
            start.Label.Should().Be("Action1");
            var next1 = start.Next.Should().BeOfType<ActionNode>().Subject;
            next1.Tag.Should().Be("2");
            next1.Label.Should().Be("Action2");
            var next2 = next1.Next.Should().BeOfType<ActionNode>().Subject;
            next2.Tag.Should().Be("3");
            next2.Label.Should().Be("Action3");
            next2.Next.Should().BeNull();
        }

        [Fact]
        public void Parse_ActionWithLabelAndTag_ReturnsWorkflowWithLabeledAndTaggedAction() {
            // Arrange
            const string script = """
                                  DoSomething :Tag: `Action Label`
                                  """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var start = result.Value.Should().BeOfType<ActionNode>().Subject;
            start.Tag.Should().Be("Tag");
            start.Label.Should().Be("Action Label");
            start.Next.Should().BeNull();
        }
    }
}
