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
            result.Should().BeNull();
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
            var start = result.Should().BeOfType<ActionNode>().Subject;
            start.Id.Should().Be(1);
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
            var start = result.Should().BeOfType<ActionNode>().Subject;
            start.Id.Should().Be(1);
            start.Tag.Should().Be("1");
            start.Label.Should().Be("DoSomething");
            var end = start.Next.Should().BeOfType<ExitNode>().Subject;
            end.Id.Should().Be(2);
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
            var start = result.Should().BeOfType<ActionNode>().Subject;
            start.Id.Should().Be(1);
            start.Tag.Should().Be("1");
            start.Label.Should().Be("Action1");
            var next1 = start.Next.Should().BeOfType<ActionNode>().Subject;
            next1.Id.Should().Be(2);
            next1.Tag.Should().Be("2");
            next1.Label.Should().Be("Action2");
            var next2 = next1.Next.Should().BeOfType<ActionNode>().Subject;
            next2.Id.Should().Be(3);
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
            var start = result.Should().BeOfType<ActionNode>().Subject;
            start.Id.Should().Be(1);
            start.Tag.Should().Be("Tag");
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
            var ifNode = result.Should().BeOfType<IfNode>().Subject;
            ifNode.Id.Should().Be(1);
            ifNode.Tag.Should().Be("1");
            ifNode.Label.Should().Be("if");
            ifNode.Next.Should().BeNull();

            var trueAction = ifNode.IsTrue.Should().BeOfType<ActionNode>().Subject;
            trueAction.Id.Should().Be(2);
            trueAction.Tag.Should().Be("2");
            trueAction.Label.Should().Be("ActionTrue");

            trueAction.Next.Should().BeNull();
            ifNode.IsFalse.Should().BeNull();
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
            var ifNode = result.Should().BeOfType<IfNode>().Subject;
            ifNode.Id.Should().Be(1);
            ifNode.Tag.Should().Be("1");
            ifNode.Label.Should().Be("if");

            var trueAction = ifNode.IsTrue.Should().BeOfType<ActionNode>().Subject;
            trueAction.Id.Should().Be(2);
            trueAction.Tag.Should().Be("2");
            trueAction.Label.Should().Be("ActionTrue");

            var end = trueAction.Next.Should().BeOfType<ExitNode>().Subject;
            end.Id.Should().Be(3);
            end.Tag.Should().Be("3");
            end.Label.Should().Be("end");
            end.ExitCode.Should().Be(0);

            ifNode.IsFalse.Should().Be(end);
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
            var ifNode = result.Should().BeOfType<IfNode>().Subject;
            ifNode.Id.Should().Be(1);
            ifNode.Tag.Should().Be("1");
            ifNode.Label.Should().Be("if");

            var trueAction = ifNode.IsTrue.Should().BeOfType<ActionNode>().Subject;
            trueAction.Id.Should().Be(2);
            trueAction.Tag.Should().Be("2");
            trueAction.Label.Should().Be("ActionTrue");

            var falseAction = ifNode.IsFalse.Should().BeOfType<ActionNode>().Subject;
            falseAction.Id.Should().Be(3);
            falseAction.Tag.Should().Be("3");
            falseAction.Label.Should().Be("ActionFalse");

            var endTrue = trueAction.Next.Should().BeOfType<ActionNode>().Subject;
            endTrue.Id.Should().Be(4);
            endTrue.Tag.Should().Be("4");
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
            var caseNode = result.Should().BeOfType<CaseNode>().Subject;
            caseNode.Id.Should().Be(1);
            caseNode.Tag.Should().Be("1");
            caseNode.Label.Should().Be("case");

            caseNode.Choices.Should().HaveCount(3);

            var option1 = caseNode.Choices["Option1"].Should().BeOfType<ActionNode>().Subject;
            option1.Id.Should().Be(2);
            option1.Tag.Should().Be("2");
            option1.Label.Should().Be("Action1");

            var option2 = caseNode.Choices["Option2"].Should().BeOfType<ActionNode>().Subject;
            option2.Id.Should().Be(3);
            option2.Tag.Should().Be("3");
            option2.Label.Should().Be("Action2");

            var otherwise = caseNode.Choices[string.Empty].Should().BeOfType<ActionNode>().Subject;
            otherwise.Id.Should().Be(4);
            otherwise.Tag.Should().Be("4");
            otherwise.Label.Should().Be("ActionDefault");

            var end = option1.Next.Should().BeOfType<ExitNode>().Subject;
            end.Id.Should().Be(5);
            end.Tag.Should().Be("5");
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
            var start = result.Should().BeOfType<ActionNode>().Subject;
            start.Id.Should().Be(1);
            start.Tag.Should().Be("1");
            start.Label.Should().Be("DoSomething");

            var end = start.Next.Should().BeOfType<ExitNode>().Subject;
            end.Id.Should().Be(2);
            end.Tag.Should().Be("2");
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
            tokens.Should().HaveCount(19);
            tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "Action1"));
            tokens[1].Should().BeEquivalentTo(new Token(TokenType.Tag, 1, 9, "Label1"));
            tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 16));
            tokens[3].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 1, "Action2"));
            tokens[4].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 7));
            tokens[5].Should().BeEquivalentTo(new Token(TokenType.If, 3, 1, "IF"));
            tokens[6].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 4, "Condition"));
            tokens[7].Should().BeEquivalentTo(new Token(TokenType.EOL, 3, 12));
            tokens[8].Should().BeEquivalentTo(new Token(TokenType.Indent, 4));
            tokens[9].Should().BeEquivalentTo(new Token(TokenType.JumpTo, 4, 3, "GOTO"));
            tokens[10].Should().BeEquivalentTo(new Token(TokenType.Identifier, 4, 8, "end"));
            tokens[11].Should().BeEquivalentTo(new Token(TokenType.EOL, 4, 10));
            tokens[12].Should().BeEquivalentTo(new Token(TokenType.JumpTo, 5, 1, "GOTO"));
            tokens[13].Should().BeEquivalentTo(new Token(TokenType.Identifier, 5, 6, "Label1"));
            tokens[14].Should().BeEquivalentTo(new Token(TokenType.EOL, 5, 11));
            tokens[15].Should().BeEquivalentTo(new Token(TokenType.Exit, 6, 1, "EXIT"));
            tokens[16].Should().BeEquivalentTo(new Token(TokenType.Tag, 6, 6, "end"));
            tokens[17].Should().BeEquivalentTo(new Token(TokenType.EOL, 6, 10));
            tokens[18].Should().BeEquivalentTo(new Token(TokenType.EOF, 6, 66));

            var action1 = result.Should().BeOfType<ActionNode>().Subject;
            action1.Id.Should().Be(1);
            action1.Tag.Should().Be("Label1");
            action1.Label.Should().Be("Action1");

            var action2 = action1.Next.Should().BeOfType<ActionNode>().Subject;
            action2.Id.Should().Be(2);
            action2.Tag.Should().Be("2");
            action2.Label.Should().Be("Action2");

            var ifNode = action2.Next.Should().BeOfType<IfNode>().Subject;
            ifNode.Id.Should().Be(3);
            ifNode.Tag.Should().Be("3");
            ifNode.Label.Should().Be("if");

            var exitJump = ifNode.IsTrue.Should().BeOfType<JumpNode>().Subject;
            exitJump.Id.Should().Be(4);
            exitJump.Tag.Should().Be("4");
            exitJump.Label.Should().Be("goto");
            exitJump.TargetTag.Should().Be("end");

            var jumpBack = ifNode.IsFalse.Should().BeOfType<JumpNode>().Subject;
            jumpBack.Id.Should().Be(5);
            jumpBack.Tag.Should().Be("5");
            jumpBack.Label.Should().Be("goto");
            jumpBack.TargetTag.Should().Be("Label1");
            jumpBack.Next.Should().Be(action1);

            var end = exitJump.Next.Should().BeOfType<ExitNode>().Subject;
            end.Id.Should().Be(6);
            end.Tag.Should().Be("end");
            end.Label.Should().Be("end");
            end.ExitCode.Should().Be(0);
        }
    }
}
