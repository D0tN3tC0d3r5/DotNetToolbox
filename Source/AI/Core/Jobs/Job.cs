namespace DotNetToolbox.AI.Jobs;

public class Job<TInput, TOutput>(string id, JobContext context, Action<TInput> updateMessages, Func<TaskResponseType, string, TOutput> mapResponse)
    : IJob<TInput, TOutput> {
    private readonly Action<TInput>? _updateMessages = updateMessages;
    private readonly Func<TaskResponseType, string, TOutput>? _mapResponse = mapResponse;

    public Job(IStringGuidProvider guid, JobContext context, Action<TInput> updateMessages, Func<TaskResponseType, string, TOutput> mapResponse)
        : this(guid.CreateSortable(), context, updateMessages, mapResponse) {
    }
    public Job(JobContext context, Action<TInput> updateMessages, Func<TaskResponseType, string, TOutput> mapResponse)
        : this(StringGuidProvider.Default, context, updateMessages, mapResponse) {
    }
    public Job(string id, JobContext context)
        : this(id, context, null!, null!) {
    }
    public Job(IStringGuidProvider guid, JobContext context)
        : this(guid.CreateSortable(), context, null!, null!) {
    }
    public Job(JobContext context)
        : this(StringGuidProvider.Default, context, null!, null!) {
    }

    public string Id { get; } = id;
    public JobType Type { get; protected init; } = JobType.Generic;

    protected JobContext Context { get; } = context;
    protected Messages Messages { get; } = new Messages(id);

    protected virtual void AppendInputMessage(TInput input) {
        if (Messages.Count == 0) Messages.SetSystemMessage(Context.Task.Prompt);
        Messages.AppendUserMessage(GenerateInputPrompt(input));
    }
    protected virtual TOutput MapResponse(TaskResponseType responseType, string response)
        => _mapResponse is null
            ? throw new NotImplementedException()
            : _mapResponse(responseType, response);

    public async Task<Result<TOutput>> Execute(TInput input, CancellationToken ct) {
        AppendInputMessage(input);
        var response = await Context.Agent.SendRequest(Messages, Context, ct);
        return response.HasErrors
            ? response.Errors
            : MapResponse(Context.Task.ResponseType, response.Value ?? string.Empty);
    }

    private string GenerateInputPrompt(TInput input) {
        var template = Context.Task.InputTemplate;

        if (string.IsNullOrEmpty(template))
            return string.Empty;

        var properties = typeof(TInput).GetProperties();
        foreach (var prop in properties) {
            var placeholder = $"[{prop.Name}]";
            if (template.Contains(placeholder)) {
                var value = prop.GetValue(input)?.ToString() ?? string.Empty;
                template = template.Replace(placeholder, value);
            }
        }

        return template;
    }
}
