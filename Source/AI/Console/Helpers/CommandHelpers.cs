namespace AI.Sample.Helpers;

public static class CommandHelpers {
    private sealed class ListItem<TItem, TKey>(object? key, string text, TItem? item)
        where TItem : class, IEntity<TKey>
        where TKey : notnull {
        public object? Key { get; set; } = key;
        public string Text { get; set; } = text;
        public TItem? Item { get; } = item;
    }

    public static TItem? EntitySelectionPrompt<TItem, TKey>(this ICommand command,
                                                      IEnumerable<TItem> entities,
                                                      string action,
                                                      string name,
                                                      Func<TItem, TKey> itemKey,
                                                      Func<TItem, string> itemText,
                                                      object? cancelKey = null,
                                                      string? cancelText = null)
        where TItem : class, IEntity<TKey>
        where TKey : notnull {
        if (!IsNotNull(entities).Any()) {
            command.Output.WriteLine($"[yellow]No {IsNotNullOrWhiteSpace(name)} available to {IsNotNullOrWhiteSpace(action)}.[/]");
            return null;
        }

        var choices = entities.Select(e => new ListItem<TItem, TKey>(IsNotNull(itemKey)(e), IsNotNull(itemText)(e), e)).ToList();
        var cancelOption = new ListItem<TItem, TKey>(cancelKey, cancelText ?? "Cancel", null);
        choices.Add(cancelOption);

        var prompt = $"Select {IndefiniteArticleFor(name[0])} {name} to {action} (or cancel):";
        return command.Input.BuildSelectionPrompt<ListItem<TItem, TKey>>(prompt)
                      .AddChoices([.. choices])
                      .ConvertWith(e => IsCancel(e) ? $"[yellow bold]{e.Text}[/]" : e.Text)
                      .Show().Item;

        bool IsCancel(ListItem<TItem, TKey> item) => item.Key?.Equals(cancelKey) ?? cancelKey is null;
        static string IndefiniteArticleFor(char c) => c is 'a' or 'e' or 'i' or 'o' or 'u' ? "an" : "a";
    }
}
