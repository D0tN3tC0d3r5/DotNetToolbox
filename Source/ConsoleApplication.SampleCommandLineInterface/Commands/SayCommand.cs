namespace DotNetToolbox.ConsoleApplication.SampleCommandLineInterface.Commands;

internal class SayCommand : Command<SayCommand> {
    public SayCommand(IHasChildren node)
        : base(node, "Say") {
        AddParameter("Text");
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        var parameter = Children.OfType<Parameter>().First(i => i.Name == "Text");
        Output.WriteLine(parameter.Value);
        return SuccessTask();
    }
}
