namespace BigMouth;

public class BigMouth
    : ShellApplication<BigMouth, ApplicationSettings> {
    public BigMouth(string[] args, IServiceCollection services)
        : base(args, services) {
        AddCommand<SayCommand>();
    }

    protected override Task<Result> OnStart(CancellationToken ct = default) {
        var result = base.OnStart(ct);
        var name = Context["MyName"];
        Output.WriteLine($"Hello {name}.");
        return result;
    }
}
