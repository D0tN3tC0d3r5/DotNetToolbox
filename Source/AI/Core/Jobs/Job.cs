using System.Collections;

namespace DotNetToolbox.AI.Jobs;

public class Job<TInput, TOutput>(string id, JobContext context, Func<TaskResponseType, string, TOutput>? mapResponse = null)
    : IJob<TInput, TOutput> {
    private readonly Chat _chat = new(id);

    public Job(IStringGuidProvider guid, JobContext context, Func<TaskResponseType, string, TOutput>? mapResponse = null)
        : this(guid.CreateSortable(), context, mapResponse) {
    }
    public Job(JobContext context)
        : this(StringGuidProvider.Default, context) {
    }
    public string Id { get; } = id;

    private void AppendInputMessage(TInput input) {
        if (_chat.Count == 0) {
            _chat.SetSystemMessage($"""
                # Agent Description
                {context.Persona.Prompt}

                # Task Description
                {context.Task.Prompt}
                """);
        }
        _chat.AppendMessage(MessageRole.User, GenerateInputPrompt(input));
    }

    private TOutput MapResponse(TaskResponseType responseType, string response)
        => mapResponse is null
            ? throw new NotImplementedException()
            : mapResponse(responseType, response);

    public async Task<Result<TOutput>> Execute(TInput input, CancellationToken ct) {
        AppendInputMessage(input);
        var response = await context.Agent.SendRequest(_chat, context, ct);
        return response.HasErrors
            ? response.Errors
            : MapResponse(context.Task.ResponseType, response.Value ?? string.Empty);
    }

    private string GenerateInputPrompt(TInput input) {
        var template = context.Task.InputTemplate;

        if (string.IsNullOrEmpty(template)) {
            return input is null ? string.Empty
                 : input is string || input.GetType().IsPrimitive ? $"{input}"
                 : JsonSerializer.Serialize(input);
        }

        foreach (var prop in typeof(TInput).GetProperties()) {
            var placeholder = $"<<{prop.Name}>>";
            if (!template.Contains(placeholder)) continue;

            var value = prop.GetValue(input);
            var valueText = value switch {
                null => "[[no value found]]",
                ICollection { Count: > 0 } arrayValue => $" - {string.Join("\n - ", [.. arrayValue])}",
                ICollection => "[[no items found]]",
                string stringValue => stringValue,
                _ => $"{value}",
            };
            template = template.Replace(placeholder, valueText);
        }

        return template;
    }
}
