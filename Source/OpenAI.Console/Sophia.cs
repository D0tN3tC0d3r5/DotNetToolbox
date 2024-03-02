using DotNetToolbox.OpenAI.Agents;

namespace DotNetToolbox.Sophia;

public class Sophia : ShellApplication<Sophia> {
    private readonly StateMachine _stateMachine;

    public Sophia(string[] args, IServiceProvider services, IAgentHandler chatHandler)
        : base(args, services) {
        AllowMultiLine = true;
        _stateMachine = new(this, chatHandler);
    }

    protected override string GetPrePromptText() => $"[{TotalNumberOfTokens}] ";

    private int TotalNumberOfTokens => _stateMachine.Mission?.TotalNumberOfTokens ?? 0;

    protected override async Task<Result> OnStart(CancellationToken ct) {
        _stateMachine.Start();
        await _stateMachine.Process(string.Empty, ct);
        return await base.OnStart(ct);
    }

    protected override async Task<Result> ExecuteDefault(CancellationToken ct) {
        await _stateMachine.Process(string.Empty, ct);
        return _stateMachine.CurrentState != 99 ? Result.Success() : await base.ExecuteDefault(ct);
    }

    protected override async Task<Result> ProcessFreeText(string[] lines, CancellationToken ct) {
        _stateMachine.CurrentState = 5;
        await _stateMachine.Process(string.Join('\n', lines), ct);
        return Result.Success();
    }

    public override void Exit(int code = IApplication.DefaultExitCode) {
        if (_stateMachine.CurrentState == 4) base.Exit(code);
        _stateMachine.CurrentState = 0;
    }
}
