namespace DotNetToolbox.Sophia;

public class Sophia : ShellApplication<Sophia> {
    private readonly StateMachine _stateMachine;

    public Sophia(string[] args, IServiceProvider services)
        : base(args, services) {
        AllowMultiLine = true;
        _stateMachine = new(this);
    }

    protected override string GetPrePromptText() => $"[{TotalNumberOfTokens}] ";

    private int TotalNumberOfTokens => _stateMachine._runner?.Chat.TotalNumberOfTokens ?? 0;

    protected override async Task<Result> OnStart(CancellationToken ct) {
        await _stateMachine.Start(1, ct);
        return await base.OnStart(ct);
    }

    protected override async Task<Result> ExecuteDefault(CancellationToken ct) {
        await _stateMachine.Process(string.Empty, ct);
        return _stateMachine.CurrentState != Idle ? Result.Success() : await base.ExecuteDefault(ct);
    }

    protected override async Task<Result> ProcessFreeText(string[] lines, CancellationToken ct) {
        _stateMachine.CurrentState = 6;
        await _stateMachine.Process(string.Join('\n', lines), ct);
        return Result.Success();
    }

    public override void Exit(int code = IApplication.DefaultExitCode) {
        if (_stateMachine.CurrentState == 5) base.Exit(code);
        _stateMachine.CurrentState = 1;
    }
}
