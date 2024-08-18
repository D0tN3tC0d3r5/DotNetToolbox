namespace DotNetToolbox.Graph.Parser;

public class WorkflowParserTests {
    private readonly IServiceProvider _mockServiceProvider;

    public WorkflowParserTests() {
        _mockServiceProvider = Substitute.For<IServiceProvider>();
    }

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
            start.Id.Should().Be("1");
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
            start.Id.Should().Be("1");
            start.Label.Should().Be("DoSomething");
            var end = start.Next.Should().BeOfType<ExitNode>().Subject;
            end.Id.Should().Be("2");
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
            start.Id.Should().Be("1");
            start.Label.Should().Be("Action1");
            var next1 = start.Next.Should().BeOfType<ActionNode>().Subject;
            next1.Id.Should().Be("2");
            next1.Label.Should().Be("Action2");
            var next2 = next1.Next.Should().BeOfType<ActionNode>().Subject;
            next2.Id.Should().Be("3");
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
            start.Id.Should().Be("Tag");
            start.Label.Should().Be("Action Label");
            start.Next.Should().BeNull();
        }
    }

    public class ControlStructureTests : WorkflowParserTests {
        [Fact]
        public void Parse_IfThen_ReturnsWorkflowWithIfStructure() {
            // Arrange
            const string script = """
                              IF Condition THEN
                                ActionTrue
                              """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var ifNode = result.Value.Should().BeOfType<IfNode>().Subject;
            ifNode.Id.Should().Be("1");
            ifNode.Label.Should().Be("if");
            ifNode.Next.Should().BeNull();

            var trueAction = ifNode.Then.Should().BeOfType<ActionNode>().Subject;
            trueAction.Id.Should().Be("2");
            trueAction.Label.Should().Be("ActionTrue");

            trueAction.Next.Should().BeNull();
            ifNode.Else.Should().BeNull();
        }

        [Fact]
        public void Parse_IfThenWithExit_ReturnsWorkflowWithIfStructure() {
            // Arrange
            const string script = """
                              IF Condition THEN
                                ActionTrue
                              EXIT
                              """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var ifNode = result.Value.Should().BeOfType<IfNode>().Subject;
            ifNode.Id.Should().Be("1");
            ifNode.Label.Should().Be("if");

            var trueAction = ifNode.Then.Should().BeOfType<ActionNode>().Subject;
            trueAction.Id.Should().Be("2");
            trueAction.Label.Should().Be("ActionTrue");

            var end = trueAction.Next.Should().BeOfType<ExitNode>().Subject;
            end.Id.Should().Be("3");
            end.Label.Should().Be("end");
            end.ExitCode.Should().Be(0);

            ifNode.Else.Should().Be(end);
        }

        [Fact]
        public void Parse_IfThenElse_ReturnsWorkflowWithIfElseStructure() {
            // Arrange
            const string script = """
                              IF Condition THEN
                                ActionTrue
                              ELSE
                                ActionFalse
                              Action1
                              """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var ifNode = result.Value.Should().BeOfType<IfNode>().Subject;
            ifNode.Id.Should().Be("1");
            ifNode.Label.Should().Be("if");

            var trueAction = ifNode.Then.Should().BeOfType<ActionNode>().Subject;
            trueAction.Id.Should().Be("2");
            trueAction.Label.Should().Be("ActionTrue");

            var falseAction = ifNode.Else.Should().BeOfType<ActionNode>().Subject;
            falseAction.Id.Should().Be("3");
            falseAction.Label.Should().Be("ActionFalse");

            var endTrue = trueAction.Next.Should().BeOfType<ActionNode>().Subject;
            endTrue.Id.Should().Be("4");
            endTrue.Label.Should().Be("Action1");

            falseAction.Next.Should().Be(endTrue);
        }

        [Fact]
        public void Parse_Case_ReturnsWorkflowWithCaseStructure() {
            // Arrange
            const string script = """
                              CASE Selection
                                IS "Option1"
                                  Action1
                                IS "Option2"
                                  Action2
                                OTHERWISE
                                  ActionDefault
                              EXIT
                              """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.Errors.Should().BeEmpty();
            result.IsSuccess.Should().BeTrue();
            var caseNode = result.Value.Should().BeOfType<CaseNode>().Subject;
            caseNode.Id.Should().Be("1");
            caseNode.Label.Should().Be("case");

            caseNode.Choices.Should().HaveCount(3);

            var option1 = caseNode.Choices["Option1"].Should().BeOfType<ActionNode>().Subject;
            option1.Id.Should().Be("2");
            option1.Label.Should().Be("Action1");

            var option2 = caseNode.Choices["Option2"].Should().BeOfType<ActionNode>().Subject;
            option2.Id.Should().Be("3");
            option2.Label.Should().Be("Action2");

            var otherwise = caseNode.Choices[string.Empty].Should().BeOfType<ActionNode>().Subject;
            otherwise.Id.Should().Be("4");
            otherwise.Label.Should().Be("ActionDefault");

            var end = option1.Next.Should().BeOfType<ExitNode>().Subject;
            end.Id.Should().Be("5");
            end.Label.Should().Be("end");
            end.ExitCode.Should().Be(0);

            option2.Next.Should().Be(end);
            otherwise.Next.Should().Be(end);
        }
    }

    public class FlowControlTests : WorkflowParserTests {
        [Fact]
        public void Parse_ExitWithCode_ReturnsWorkflowWithExitCode() {
            // Arrange
            const string script = """
                              DoSomething
                              EXIT 13
                              """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var start = result.Value.Should().BeOfType<ActionNode>().Subject;
            start.Id.Should().Be("1");
            start.Label.Should().Be("DoSomething");

            var end = start.Next.Should().BeOfType<ExitNode>().Subject;
            end.Id.Should().Be("2");
            end.Label.Should().Be("end");
            end.ExitCode.Should().Be(13);
        }

        [Fact]
        public void Parse_JumpTo_ReturnsWorkflowWithJump() {
            // Arrange
            const string script = """
                              Action1 :Label1:
                              Action2
                              IF Condition
                                GOTO end
                              GOTO Label1
                              EXIT :end:
                              """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var action1 = result.Value.Should().BeOfType<ActionNode>().Subject;
            action1.Id.Should().Be("Label1");
            action1.Label.Should().Be("Action1");

            var action2 = action1.Next.Should().BeOfType<ActionNode>().Subject;
            action2.Id.Should().Be("2");
            action2.Label.Should().Be("Action2");

            var ifNode = action2.Next.Should().BeOfType<IfNode>().Subject;
            ifNode.Id.Should().Be("3");
            ifNode.Label.Should().Be("if");

            var exitJump = ifNode.Then.Should().BeOfType<JumpNode>().Subject;
            exitJump.Id.Should().Be("4");
            exitJump.Label.Should().Be("goto");
            exitJump.TargetTag.Should().Be("end");

            var jumpBack = ifNode.Else.Should().BeOfType<JumpNode>().Subject;
            jumpBack.Id.Should().Be("5");
            jumpBack.Label.Should().Be("goto");
            jumpBack.TargetTag.Should().Be("Label1");
            jumpBack.Next.Should().Be(action1);

            var end = exitJump.Next.Should().BeOfType<ExitNode>().Subject;
            end.Id.Should().Be("end");
            end.Label.Should().Be("end");
            end.ExitCode.Should().Be(0);
        }
    }

    public class ParsingErrorTests : WorkflowParserTests {
        [Fact]
        public void Parse_InvalidToken_ReturnsErrorResult() {
            // Arrange
            const string script = "Invalid$Token";
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeFalse();
            var error = result.Errors.Should().ContainSingle().Subject;
            error.Message.Should().Be("Identifier can only contain letters, numbers, and underscores.");
            error.Source.Should().Be("[1, 1]: Error");
        }

        [Fact]
        public void Parse_MissingEndOfLine_ReturnsErrorResult() {
            // Arrange
            const string script = "Action1 Action2";
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeFalse();
            var error = result.Errors.Should().ContainSingle().Subject;
            error.Message.Should().Be("Expected token: 'EndOfLine'.");
            error.Source.Should().Be("[1, 9]: Identifier");
        }

        [Fact]
        public void Parse_IncompleteIf_ReturnsErrorResult() {
            // Arrange
            const string script = "IF Condition";
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeFalse();
            var error = result.Errors.Should().ContainSingle().Subject;
            error.Message.Should().Be("Missing true condition branch.");
            error.Source.Should().Be("[1, 12]: EndOfFile");
        }

        [Fact]
        public void Parse_InvalidCaseStructure_ReturnsErrorResult() {
            // Arrange
            const string script = """
                                  CASE Selection
                                    IS "Option1"
                                      Action1
                                    IS "Option2"
                                      Action2
                                    ELSE
                                      ActionDefault
                                  """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeFalse();
            var error = result.Errors.Should().ContainSingle().Subject;
            error.Message.Should().Be("Unexpected token.");
            error.Source.Should().Be("[6, 3]: Else");
        }

        [Fact]
        public void Parse_MissingExitCode_ReturnsErrorResult() {
            // Arrange
            const string script = "EXIT InvalidCode";
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeFalse();
            var error = result.Errors.Should().ContainSingle().Subject;
            error.Message.Should().Be("Expected token: 'EndOfLine'.");
            error.Source.Should().Be("[1, 6]: Identifier");
        }

        [Fact]
        public void Parse_InvalidJumpTarget_ReturnsErrorResult() {
            // Arrange
            const string script = "GOTO 123";
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeFalse();
            var error = result.Errors.Should().ContainSingle().Subject;
            error.Message.Should().Be("Expected token: 'EndOfLine'.");
            error.Source.Should().Be("[1, 6]: Identifier");

            result.Errors.Should().ContainSingle()
                .Which.Should().Be("Number @ (1, 6): Expected token: 'Identifier'.");
        }

        [Fact]
        public void Parse_MultipleErrors_ReturnsAllErrors() {
            // Arrange
            const string script = """
                                  InvalidToken
                                  IF Condition
                                    Action1
                                  CASE Selection
                                    IS "Option1"
                                      Action2
                                    ELSE
                                      ActionDefault
                                  """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
            result.Errors.Should().Contain("Identifier @ (1, 1): Unexpected token.");
            result.Errors.Should().Contain("EndOfLine @ (2, 13): Expected token: 'Then'.");
            result.Errors.Should().Contain("Identifier @ (8, 1): Expected token: 'EndOfLine'.");
        }

        [Fact]
        public void Parse_EmptyCase_ReturnsErrorResult() {
            // Arrange
            const string script = """
                                  CASE Selection
                                  """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .Which.Should().Be("EndOfFile @ (2, 1): Token not allowed.");
        }

        [Fact]
        public void Parse_DuplicateOtherwise_ReturnsErrorResult() {
            // Arrange
            const string script = """
                                  CASE Selection
                                    IS "Option1"
                                      Action1
                                    OTHERWISE
                                      ActionDefault1
                                    OTHERWISE
                                      ActionDefault2
                                  """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .Which.Should().Be("Otherwise @ (7, 1): Token not allowed.");
        }

        [Fact]
        public void Parse_InvalidIndentation_ReturnsErrorResult() {
            // Arrange
            const string script = """
                                  IF Condition THEN
                                  Action1
                                  """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .Which.Should().Be("Identifier @ (2, 1): Error creating conditional node: Invalid indentation.");
        }
    }
}
