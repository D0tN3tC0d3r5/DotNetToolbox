namespace BigMouth;

public class BigMouth
    : ShellApplication<BigMouth> {
    public BigMouth(string[] args, IServiceProvider services)
        : base(args, services) {
        AddCommand<SayCommand>();
    }

    protected override Task<Result> Execute(CancellationToken ct) {
        var name = Context["MyName"];
        Environment.Output.WriteLine($"Hello {name}.");
        return base.Execute(ct);
    }
}
