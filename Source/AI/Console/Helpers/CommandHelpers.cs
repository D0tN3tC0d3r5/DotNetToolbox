using System.Diagnostics.CodeAnalysis;

using ILogger = Microsoft.Extensions.Logging.ILogger;
using Task = System.Threading.Tasks.Task;

namespace AI.Sample.Helpers;

public static class CommandHelpers {
    private sealed class ListItem<TItem, TKey>(object? key, string text, TItem? item)
        where TItem : class, IEntity<TKey>
        where TKey : notnull {
        public object? Key { get; } = key;
        public string Text { get; } = text;
        public TItem? Item { get; } = item;
    }

    public static async Task<TItem?> SelectEntityAsync<TItem, TKey>(this ICommand command,
                                                                     IEnumerable<TItem> entities,
                                                                     string action,
                                                                     string name,
                                                                     Func<TItem, TKey> itemKey,
                                                                     Func<TItem, string> itemText,
                                                                     CancellationToken ct = default)
        where TItem : class, IEntity<TKey>
        where TKey : notnull {
        var items = IsNotNull(entities).ToArray();
        if (items.Length == 0) {
            command.Output.WriteLine($"[yellow]No {IsNotNullOrWhiteSpace(name)} available to {IsNotNullOrWhiteSpace(action)}.[/]");
            return null;
        }

        var choices = items.Select(e => new ListItem<TItem, TKey>(IsNotNull(itemKey)(e), IsNotNull(itemText)(e), e)).ToList();
        var cancelOption = new ListItem<TItem, TKey>(null, "Cancel", null);
        choices.Add(cancelOption);

        var prompt = $"Select {IndefiniteArticleFor(name[0])} {name} to {action} (or cancel):";
        return (await command.Input.BuildSelectionPrompt<ListItem<TItem, TKey>>(prompt)
                                   .AddChoices([.. choices])
                                   .ConvertWith(e => e.Key is null ? $"[yellow bold]{e.Text}[/]" : e.Text)
                                   .ShowAsync(ct)).Item;

        static string IndefiniteArticleFor(char c) => c is 'a' or 'e' or 'i' or 'o' or 'u' ? "an" : "a";
    }

    public static Result HandleCommand(this ICommand command, Func<Result> execute, string errorMessage) {
        try {
            return execute();
        }
        catch (Exception ex) {
            return command.HandleException(ex, errorMessage);
        }
    }

    public static Task<Result> HandleCommandAsync(this ICommand command, Func<CancellationToken, Task<Result>> execute, string errorMessage, CancellationToken ct = default) {
        try {
            return execute(ct);
        }
        catch (Exception ex) {
            return Task.FromResult(command.HandleException(ex, errorMessage));
        }
    }

    private static Result HandleException(this ICommand command, Exception ex, [StringSyntax("CompositeFormat")] string message, params object[] args) {
        command.Logger.LogError(ex, message, args);
        command.Output.WriteError(ex, string.Format(message, args));
        command.Output.WriteLine();
        return Result.Error(ex);
    }
}
