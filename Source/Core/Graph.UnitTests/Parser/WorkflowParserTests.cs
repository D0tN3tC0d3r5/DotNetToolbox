namespace DotNetToolbox.Graph.Parser;

public partial class WorkflowParserTests {
    private readonly IServiceProvider _mockServiceProvider;

    public WorkflowParserTests() {
        _mockServiceProvider = Substitute.For<IServiceProvider>();
    }
}
