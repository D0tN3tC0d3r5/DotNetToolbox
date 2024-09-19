namespace Lola.Utilities;

internal sealed class ListItem<TItem, TKey>(TKey key, string text, TItem? item)
    where TItem : class, IEntity<TKey>
    where TKey : notnull {
    public TKey Key { get; } = key;
    public string Text { get; } = text;
    public TItem? Item { get; } = item;
}

public static class CommandHelpers {
    public static async Task<TItem?> SelectEntityAsync<TItem, TKey>(this ICommand command,
                                                                    IEnumerable<TItem> entities,
                                                                    Func<TItem, string> mapText,
                                                                    CancellationToken ct = default)
        where TItem : class, IEntity<TKey>
        where TKey : notnull {
        var items = IsNotNull(entities).ToArray();
        if (items.Length == 0) {
            command.Output.WriteLine("[yellow]No items found.[/]");
            return null;
        }

        var choices = items.ToList(e => new ListItem<TItem, TKey>(e.Id, IsNotNull(mapText)(e), e));
        var cancelOption = new ListItem<TItem, TKey>(default!, "Cancel", null);
        choices.Add(cancelOption);

        const string prompt = "Select an item or cancel to return:";
        return (await command.Input.BuildSelectionPrompt<ListItem<TItem, TKey>>(prompt)
                                   .AddChoices([.. choices])
                                   .ConvertWith(e => e.Item is null ? $"[yellow bold]{e.Text}[/]" : e.Text)
                                   .ShowAsync(ct)).Item;
    }

    public static Result HandleCommand(this ICommand command, Func<Result> execute, string errorMessage) {
        try {
            var result = execute();
            command.Output.WriteLine();
            return result;
        }
        catch (Exception ex) {
            return command.HandleException(ex, errorMessage);
        }
    }

    public static async Task<Result> HandleCommandAsync(this ICommand command, Func<CancellationToken, Task<Result>> execute, string errorMessage, CancellationToken ct = default) {
        try {
            var result = await execute(ct);
            command.Output.WriteLine();
            return result;
        }
        catch (Exception ex) {
            return command.HandleException(ex, errorMessage);
        }
    }

    private static Result HandleException(this INode command, Exception ex, [StringSyntax("CompositeFormat")] string message, params object[] args) {
#pragma warning disable CA2254
        command.Logger.LogError(ex, message, args);
#pragma warning restore CA2254
        command.Output.WriteError(ex, string.Format(message, args));
        command.Output.WriteLine();
        return Result.Error(ex);
    }
}
