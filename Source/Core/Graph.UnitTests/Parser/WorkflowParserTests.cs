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
            var end = start.Next.Should().BeOfType<TerminalNode>().Subject;
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
                                  EXIT
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
            var end = next2.Next.Should().BeOfType<TerminalNode>().Subject;
            end.Id.Should().Be(4);
            end.Tag.Should().Be("4");
            end.Label.Should().Be("end");
            end.ExitCode.Should().Be(0);
        }

        [Fact]
        public void Parse_ActionWithLabelAndTag_ReturnsWorkflowWithLabeledAndTaggedAction() {
            // Arrange
            const string script = """
                                  DoSomething :Tag: `Action Label`
                                  EXIT
                                  """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            var start = result.Should().BeOfType<ActionNode>().Subject;
            start.Id.Should().Be(1);
            start.Tag.Should().Be("Tag");
            start.Label.Should().Be("Action Label");
            var end = start.Next.Should().BeOfType<TerminalNode>().Subject;
            end.Id.Should().Be(2);
            end.Tag.Should().Be("2");
            end.Label.Should().Be("end");
            end.ExitCode.Should().Be(0);
        }
    }

    public class ControlStructureTests : WorkflowParserTests {
        [Fact]
        public void Parse_IfThen_ReturnsWorkflowWithIfStructure() {
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
            var ifNode = result.Should().BeOfType<ConditionalNode>().Subject;
            ifNode.Id.Should().Be(1);
            ifNode.Tag.Should().Be("1");
            ifNode.Label.Should().Be("if");

            var trueAction = ifNode.IsTrue.Should().BeOfType<ActionNode>().Subject;
            trueAction.Id.Should().Be(2);
            trueAction.Tag.Should().Be("2");
            trueAction.Label.Should().Be("ActionTrue");

            var end = ifNode.Next.Should().BeOfType<TerminalNode>().Subject;
            end.Id.Should().Be(3);
            end.Tag.Should().Be("3");
            end.Label.Should().Be("end");
            end.ExitCode.Should().Be(0);
        }

        [Fact]
        public void Parse_IfThenElse_ReturnsWorkflowWithIfElseStructure() {
            // Arrange
            const string script = """
                              IF Condition
                                THEN
                                  ActionTrue
                                ELSE
                                  ActionFalse
                              EXIT
                              """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            var ifNode = result.Should().BeOfType<ConditionalNode>().Subject;
            ifNode.Id.Should().Be(1);
            ifNode.Tag.Should().Be("1");
            ifNode.Label.Should().Be("Condition");

            var trueAction = ifNode.IsTrue.Should().BeOfType<ActionNode>().Subject;
            trueAction.Id.Should().Be(2);
            trueAction.Tag.Should().Be("2");
            trueAction.Label.Should().Be("ActionTrue");

            var falseAction = ifNode.IsFalse.Should().BeOfType<ActionNode>().Subject;
            falseAction.Id.Should().Be(3);
            falseAction.Tag.Should().Be("3");
            falseAction.Label.Should().Be("ActionFalse");

            var endTrue = trueAction.Next.Should().BeOfType<TerminalNode>().Subject;
            endTrue.Id.Should().Be(4);
            endTrue.Tag.Should().Be("4");
            endTrue.Label.Should().Be("end");
            endTrue.ExitCode.Should().Be(0);

            var endFalse = falseAction.Next.Should().BeOfType<TerminalNode>().Subject;
            endFalse.Id.Should().Be(4);
            endFalse.Tag.Should().Be("4");
            endFalse.Label.Should().Be("end");
            endFalse.ExitCode.Should().Be(0);
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
            var caseNode = result.Should().BeOfType<BranchingNode>().Subject;
            caseNode.Id.Should().Be(1);
            caseNode.Tag.Should().Be("1");
            caseNode.Label.Should().Be("Selection");

            caseNode.Choices.Should().HaveCount(3);

            var option1 = caseNode.Choices["Option1"].Should().BeOfType<ActionNode>().Subject;
            option1.Id.Should().Be(2);
            option1.Tag.Should().Be("2");
            option1.Label.Should().Be("Action1");

            var option2 = caseNode.Choices["Option2"].Should().BeOfType<ActionNode>().Subject;
            option2.Id.Should().Be(3);
            option2.Tag.Should().Be("3");
            option2.Label.Should().Be("Action2");

            var otherwise = caseNode.Choices["OTHERWISE"].Should().BeOfType<ActionNode>().Subject;
            otherwise.Id.Should().Be(4);
            otherwise.Tag.Should().Be("4");
            otherwise.Label.Should().Be("ActionDefault");

            var end = option1.Next.Should().BeOfType<TerminalNode>().Subject;
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
                              EXIT 42
                              """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            var start = result.Should().BeOfType<ActionNode>().Subject;
            start.Id.Should().Be(1);
            start.Tag.Should().Be("1");
            start.Label.Should().Be("DoSomething");

            var end = start.Next.Should().BeOfType<TerminalNode>().Subject;
            end.Id.Should().Be(2);
            end.Tag.Should().Be("2");
            end.Label.Should().Be("end");
            end.ExitCode.Should().Be(42);
        }

        [Fact]
        public void Parse_JumpTo_ReturnsWorkflowWithJump() {
            // Arrange
            const string script = """
                              Action1 :Label1:
                              Action2
                              GOTO Label1
                              EXIT
                              """;
            var tokens = WorkflowLexer.Tokenize(script).ToList();

            // Act
            var result = WorkflowParser.Parse(tokens, _mockServiceProvider);

            // Assert
            var action1 = result.Should().BeOfType<ActionNode>().Subject;
            action1.Id.Should().Be(1);
            action1.Tag.Should().Be("Label1");
            action1.Label.Should().Be("Action1");

            var action2 = action1.Next.Should().BeOfType<ActionNode>().Subject;
            action2.Id.Should().Be(2);
            action2.Tag.Should().Be("2");
            action2.Label.Should().Be("Action2");

            var jump = action2.Next;
            jump.Should().Be(action1);

            var end = action2.Next.Should().BeOfType<TerminalNode>().Subject;
            end.Id.Should().Be(4);
            end.Tag.Should().Be("4");
            end.Label.Should().Be("end");
            end.ExitCode.Should().Be(0);
        }
    }
}
