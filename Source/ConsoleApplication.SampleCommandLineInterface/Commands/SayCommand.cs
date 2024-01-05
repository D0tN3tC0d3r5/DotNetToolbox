namespace DotNetToolbox.ConsoleApplication.SampleCommandLineInterface.Commands;

internal class SayCommand : Command<SayCommand> {
    public SayCommand(IHasChildren node)
        : base(node, "Say") {
        AddParameter<string>("Text");
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        var parameter = Children.OfType<Parameter<string>>().First(i => i.Name == "Text");
        Output.WriteLine(parameter.Value);
        return SuccessTask();
    }
}
