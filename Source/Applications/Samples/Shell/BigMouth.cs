namespace BigMouth;

public class BigMouth
    : ShellApplication<BigMouth> {
    public BigMouth(string[] args, IServiceProvider services)
        : base(args, services) {
        AddCommand<SayCommand>();
    }

    protected override Task<Result> OnStart(CancellationToken ct = default) {
        var result = base.OnStart(ct);
        var name = Context["MyName"];
        Environment.OperatingSystem.Output.WriteLine($"Hello {name}.");
        return result;
    }
}
