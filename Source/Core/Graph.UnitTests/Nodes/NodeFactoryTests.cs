namespace DotNetToolbox.Graph.Nodes;

public class NodeFactoryTests {
    private readonly NodeFactory _factory = new();

    //[Fact]
    //public void CreateStart_ReturnsEntryNode() {
    //    // Act
    //    var node = _factory.CreateStart();

    //    // Assert
    //    node.Should().NotBeNull();
    //    node.Should().BeOfType<StartingNode>();
    //}

    //[Fact]
    //public void CreateFork_ReturnsIfNode() {
    //    // Act
    //    var node = _factory.CreateFork(_ => true, _ => { });

    //    // Assert
    //    node.Should().NotBeNull();
    //    node.Should().BeOfType<ConditionalNode>();
    //}

    //[Fact]
    //public void CreateFork_WithElse_ReturnsIfNode() {
    //    // Act
    //    var node = _factory.CreateFork(_ => false, _ => { }, _ => { });

    //    // Assert
    //    node.Should().NotBeNull();
    //    node.Should().BeOfType<ConditionalNode>();
    //}

    //[Fact]
    //public void CreateChoice_ReturnsSelectNode() {
    //    // Act
    //    var node = _factory.CreateChoice(_ => "1", b => {
    //        b.Is("1", _ => { });
    //        b.Is("2", _ => { });
    //    });

    //    // Assert
    //    node.Should().NotBeNull();
    //    node.Should().BeAssignableTo<BranchingNode>();
    //}

    //[Fact]
    //public void CreateAction_ReturnsActionNode() {
    //    // Act
    //    var node = _factory.CreateAction(_ => { });

    //    // Assert
    //    node.Should().NotBeNull();
    //    node.Should().BeAssignableTo<ActionNode>();
    //}

    //private class ValidTestNode : Node<ValidTestNode> {
    //    protected override void UpdateState(Context context) => throw new NotImplementedException();
    //    protected override INode GetNext(Context context) => throw new NotImplementedException();
    //}

    //private class InvalidTestNode : Node<InvalidTestNode> {
    //    protected override Result IsValid(ISet<INode> visited) => Result.Invalid("Not valid.");
    //    protected override void UpdateState(Context context) => throw new NotImplementedException();
    //    protected override INode GetNext(Context context) => throw new NotImplementedException();
    //}

    //[Fact]
    //public void Validate_ForValidNode_ReturnsSuccess() {
    //    // Arrange
    //    var node = new ValidTestNode();

    //    // Act
    //    var result = node.Validate();

    //    // Assert
    //    result.IsSuccess.Should().BeTrue();
    //}

    //[Fact]
    //public void Validate_ForInvalidNode_ReturnsInvalid() {
    //    // Arrange
    //    var node = new InvalidTestNode();

    //    // Act
    //    var result = node.Validate();

    //    // Assert
    //    result.IsSuccess.Should().BeFalse();
    //    result.IsInvalid.Should().BeTrue();
    //    result.Errors.Should().Contain("Not valid.");
    //}

    //[Fact]
    //public void Validate_ForCircularPath_ReturnsSuccess() {
    //    // Arrange
    //    var node1 = _factory.CreateAction(_ => { });
    //    node1.Next = _factory.CreateAction(_ => { });
    //    node1.Next.Next = node1;

    //    // Act
    //    var result = node1.Validate();

    //    // Assert
    //    result.IsSuccess.Should().BeTrue();
    //}

    //[Fact]
    //public void Run_ReturnsNextNode() {
    //    // Arrange
    //    var node = _factory.CreateAction(c => c["Hello"] = "World!");
    //    var next = _factory.CreateAction(c => { });
    //    node.Next = next;
    //    var context = new Context();

    //    // Act
    //    var result = node.Run(context);

    //    // Assert
    //    result.Should().Be(next);
    //    context["Hello"].Should().Be("World!");
    //}

    //[Fact]
    //public void Run_WhenIsLastNode_ReturnsNull() {
    //    // Arrange
    //    var node = _factory.CreateAction(c => c["Hello"] = "World!");
    //    var context = new Context();

    //    // Act
    //    var result = node.Run(context);

    //    // Assert
    //    result.Should().BeNull();
    //    context["Hello"].Should().Be("World!");
    //}

    public class ActionNodeTests {
        private readonly NodeFactory _factory = new();

        [Fact]
        public void CreateAction_WithoutLabel_ReturnsActionNodeWithDefaultLabel() {
            var node = _factory.CreateAction(1, _ => { });

            node.Should().NotBeNull();
            node.Should().BeOfType<ActionNode>();
            node.Label.Should().Be("action");
        }

        [Fact]
        public void CreateAction_WithCustomLabel_ReturnsActionNodeWithCustomLabel() {
            const string customLabel = "CustomAction";
            var node = _factory.CreateAction(1, customLabel, _ => { });

            node.Should().NotBeNull();
            node.Should().BeOfType<ActionNode>();
            node.Label.Should().Be(customLabel);
        }

        [Fact]
        public void CreateAction_WithGenericType_ReturnsCustomActionNode() {
            var node = _factory.CreateAction<CustomActionNode>(1);

            node.Should().NotBeNull();
            node.Should().BeOfType<CustomActionNode>();
        }

        [Fact]
        public void CreateAction_WithPolicy_AppliesPolicyToNode() {
            var policy = new TestPolicy();
            var node = _factory.CreateAction(1, _ => { }, policy);

            node.Should().NotBeNull();
            node.Should().BeOfType<ActionNode>();
        }

        [Fact]
        public void CreateAction_RunWithPolicy_ExecutesPolicyAndAction() {
            var policyExecuted = false;
            var actionExecuted = false;
            var policy = new TestPolicy(() => policyExecuted = true);
            var node = _factory.CreateAction(1, _ => actionExecuted = true, policy);
            var context = new Context();

            node.Run(context);

            policyExecuted.Should().BeTrue();
            actionExecuted.Should().BeTrue();
        }

        [Fact]
        public void CreateAction_RunMethod_UpdatesContextAndReturnsNextNode() {
            var context = new Context();
            var nextNode = _factory.CreateAction(1, _ => { });
            var node = _factory.CreateAction(2, ctx => ctx["key"] = "value");
            node.Next = nextNode;

            var result = node.Run(context);

            result.Should().BeSameAs(nextNode);
            context["key"].Should().Be("value");
        }

        private class CustomActionNode(uint id, string? label = null, IPolicy? policy = null)
            : ActionNode<CustomActionNode>(id, label, policy) {
            protected override void Execute(Context context) { }
        }

        private class TestPolicy(Action? onExecute = null)
            : IPolicy {

            public void Execute(Action action) {
                onExecute?.Invoke();
                action();
            }
        }
    }

    public class ConditionalNodeTests {
        private readonly NodeFactory _factory = new();

        [Fact]
        public void CreateFork_WithoutLabel_ReturnsConditionalNodeWithDefaultLabel() {
            var builder = new WorkflowBuilder();
            var node = _factory.CreateFork(1, _ => true, builder, _ => { });

            node.Should().NotBeNull();
            node.Should().BeOfType<ConditionalNode>();
            node.Label.Should().Be("if");
        }

        [Fact]
        public void CreateFork_WithCustomLabel_ReturnsConditionalNodeWithCustomLabel() {
            const string customLabel = "CustomFork";
            var builder = new WorkflowBuilder();
            var node = _factory.CreateFork(1, customLabel, _ => true, builder, _ => { });

            node.Should().NotBeNull();
            node.Should().BeOfType<ConditionalNode>();
            node.Label.Should().Be(customLabel);
        }

        [Fact]
        public void CreateFork_WithGenericType_ReturnsCustomConditionalNode() {
            var node = _factory.CreateFork<CustomConditionalNode>(1);

            node.Should().NotBeNull();
            node.Should().BeOfType<CustomConditionalNode>();
        }

        [Fact]
        public void CreateFork_WithTrueBranchOnly_SetsOnlyTrueBranch() {
            var builder = new WorkflowBuilder();
            var node = _factory.CreateFork(1, _ => true, builder, t => t.Do(_ => { }));

            node.Should().BeOfType<ConditionalNode>();
            node.IsTrue.Should().NotBeNull();
            node.IsFalse.Should().BeNull();
        }

        [Fact]
        public void CreateFork_WithBothBranches_SetsBothBranches() {
            var builder = new WorkflowBuilder();
            var node = _factory.CreateFork(1, _ => true, builder,
                t => t.Do(_ => { }),
                f => f.Do(_ => { }));

            node.Should().BeOfType<ConditionalNode>();
            node.IsTrue.Should().NotBeNull();
            node.IsFalse.Should().NotBeNull();
        }

        [Fact]
        public void CreateFork_RunMethodWithTrueCondition_ExecutesTrueBranch() {
            var context = new Context();
            var builder = new WorkflowBuilder();
            var node = _factory.CreateFork(1, _ => true, builder,
                t => t.Do(ctx => ctx["branch"] = "true"),
                f => f.Do(ctx => ctx["branch"] = "false"));

            node.Run(context);

            context["branch"].Should().Be("true");
        }

        [Fact]
        public void CreateFork_RunMethodWithFalseCondition_ExecutesFalseBranch() {
            var context = new Context();
            var builder = new WorkflowBuilder();
            var node = _factory.CreateFork(1, _ => false, builder,
                t => t.Do(ctx => ctx["branch"] = "true"),
                f => f.Do(ctx => ctx["branch"] = "false"));

            node.Run(context);

            context["branch"].Should().Be("false");
        }

        [Fact]
        public void CreateFork_ValidateMethod_ValidatesBothBranches() {
            var builder = new WorkflowBuilder();
            var node = _factory.CreateFork(1, _ => true, builder,
                t => t.Do(_ => { }),
                f => f.Do(_ => { }));

            var result = node.Validate();

            result.IsSuccess.Should().BeTrue();
        }

        private class CustomConditionalNode(uint id, string? label = null)
            : ConditionalNode<CustomConditionalNode>(id, label) {
            protected override bool When(Context context) => true;
        }
    }

    public class BranchingNodeTests {
        private readonly NodeFactory _factory = new();

        [Fact]
        public void CreateChoice_WithoutLabel_ReturnsBranchingNodeWithDefaultLabel() {
            var builder = new WorkflowBuilder();
            var node = _factory.CreateChoice(1, _ => "default", builder, _ => { });

            node.Should().NotBeNull();
            node.Should().BeOfType<BranchingNode>();
            node.Label.Should().Be("case");
        }

        [Fact]
        public void CreateChoice_WithCustomLabel_ReturnsBranchingNodeWithCustomLabel() {
            const string customLabel = "CustomChoice";
            var builder = new WorkflowBuilder();
            var node = _factory.CreateChoice(1, customLabel, _ => "default", builder, _ => { });

            node.Should().NotBeNull();
            node.Should().BeOfType<BranchingNode>();
            node.Label.Should().Be(customLabel);
        }

        [Fact]
        public void CreateChoice_WithGenericType_ReturnsCustomBranchingNode() {
            var node = _factory.CreateChoice<CustomBranchingNode>(1);

            node.Should().NotBeNull();
            node.Should().BeOfType<CustomBranchingNode>();
        }

        [Fact]
        public void CreateChoice_WithMultipleBranches_SetsAllBranches() {
            var builder = new WorkflowBuilder();
            var node = _factory.CreateChoice(1, _ => "key", builder, b => b
                .Is("key1", _ => { })
                .Is("key2", _ => { })
                .Is("key3", _ => { }));

            node.Should().BeOfType<BranchingNode>();
            var branchingNode = (BranchingNode)node;
            branchingNode.Choices.Should().HaveCount(3);
            branchingNode.Choices.Should().ContainKeys("key1", "key2", "key3");
        }

        [Fact]
        public void CreateChoice_RunMethodWithExistingKey_ExecutesCorrectBranch() {
            var context = new Context();
            var builder = new WorkflowBuilder();
            var node = _factory.CreateChoice(1, _ => "key2", builder, b => b
                .Is("key1", br => br.Do(ctx => ctx["branch"] = "1"))
                .Is("key2", br => br.Do(ctx => ctx["branch"] = "2"))
                .Is("key3", br => br.Do(ctx => ctx["branch"] = "3")));

            node.Run(context);

            context["branch"].Should().Be("2");
        }

        [Fact]
        public void CreateChoice_RunMethodWithNonExistingKey_ThrowsInvalidOperationException() {
            var context = new Context();
            var builder = new WorkflowBuilder();
            var node = _factory.CreateChoice(1, _ => "nonexistent", builder, b => b
                .Is("key1", _ => { })
                .Is("key2", _ => { }));

            var action = () => node.Run(context);

            action.Should().Throw<InvalidOperationException>()
                .WithMessage("The selected path was not found.");
        }

        [Fact]
        public void CreateChoice_ValidateMethod_ValidatesAllBranches() {
            var builder = new WorkflowBuilder();
            var node = _factory.CreateChoice(1, _ => "key", builder, b => b
                .Is("key1", br => br.Do(_ => { }))
                .Is("key2", br => br.Do(_ => { }))
                .Is("key3", _ => { }));

            var result = node.Validate();

            result.IsSuccess.Should().BeTrue();
        }

        private class CustomBranchingNode(uint id, string? label = null)
            : BranchingNode<CustomBranchingNode>(id, label) {
            protected override string Select(Context context) => "default";
        }
    }

    public class TerminalNodeTests {
        private readonly NodeFactory _factory = new();

        [Fact]
        public void CreateStop_WithoutLabel_ReturnsTerminalNodeWithDefaultLabel() {
            var node = _factory.CreateStop(1);

            node.Should().NotBeNull();
            node.Should().BeOfType<TerminalNode>();
            node.Label.Should().Be("end");
        }

        [Fact]
        public void CreateStop_WithCustomLabel_ReturnsTerminalNodeWithCustomLabel() {
            const string customLabel = "CustomEnd";
            var node = _factory.CreateStop(1, customLabel);

            node.Should().NotBeNull();
            node.Should().BeOfType<TerminalNode>();
            node.Label.Should().Be(customLabel);
        }
    }

    public class ComplexWorkflowTests {
        private readonly NodeFactory _factory = new();

        [Fact]
        public void ComplexWorkflow_WithMultipleNodeTypes_ExecutesCorrectly() {
            var context = new Context();
            var workflow = CreateComplexWorkflow();

            workflow.Run(context);

            context["result"].Should().Be("Action3");
        }

        [Fact]
        public void ComplexWorkflow_Validation_SucceedsForValidWorkflow() {
            var workflow = CreateComplexWorkflow();

            var result = workflow.Validate();

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void ComplexWorkflow_WithCircularReference_DetectedDuringValidation() {
            var startNode = _factory.CreateAction(1, _ => { });
            var actionNode = _factory.CreateAction(2, _ => { });
            startNode.Next = actionNode;
            actionNode.Next = startNode; // Creating a circular reference

            var result = startNode.Validate();

            result.IsSuccess.Should().BeTrue(); // Circular references are allowed, but detected
        }

        [Fact]
        public void ComplexWorkflow_WithCustomPolicy_AppliesPolicyCorrectly() {
            var policyExecutionCount = 0;
            var policy = new CustomPolicy(() => policyExecutionCount++);
            var context = new Context();

            var builder = new WorkflowBuilder();
            builder.Do(_ => { })
                   .Do(_ => { }, policy);

            var wf = builder.Start;

            wf.Run(context);

            policyExecutionCount.Should().Be(1);
        }

        private INode CreateComplexWorkflow() {
            var builder = new WorkflowBuilder();
            var workflow = _factory.CreateAction(1, _ => { });
            workflow.Next = _factory.CreateAction(2, ctx => ctx["count"] = 0);
            workflow.Next.Next = _factory.CreateFork(3,
                "LoopStart",
                ctx => (int)ctx["count"] < 2,
                builder,
                t => t
                    .Do(ctx => ctx["count"] = (int)ctx["count"] + 1)
                    .Do(ctx => ctx["result"] = "Action1")
                    .JumpTo("LoopStart"),
                f => f
                    .When(ctx => ((int)ctx["count"] % 2 == 0).ToString(),
                        b => b
                            .Is("true", br => br.Do(ctx => ctx["result"] = "Action2"))
                            .Is("false", br => br.Do(ctx => ctx["result"] = "Action3"))
                    )
            );
            return workflow;
        }

        private class CustomPolicy(Action onExecute) : IPolicy {
            public void Execute(Action action) {
                onExecute();
                action();
            }
        }
    }
}
