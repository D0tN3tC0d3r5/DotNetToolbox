namespace DotNetToolbox;

public class Context : Context<object?>;

public interface IContext : IDisposable {
    void BuildDescription(StringBuilder builder, string? myKey = null, uint level = 0);
}

public class Context<TValue>
    : ConcurrentDictionary<string, TValue>, IContext {
    private bool _isDisposed;

    protected virtual void Dispose(bool disposing) {
        if (_isDisposed) return;
        if (disposing) {
            foreach (var disposable in Values.OfType<IDisposable>()) disposable.Dispose();
        }

        _isDisposed = true;
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public override string ToString() {
        var builder = new StringBuilder();
        BuildDescription(builder);
        return builder.ToString();
    }

    public virtual void BuildDescription(StringBuilder builder, string? myKey = null, uint level = 0) {
        if (Keys.Count == 0) return;
        var indent = new string(' ', (int)level * 4);
        if (!string.IsNullOrWhiteSpace(myKey))
            builder.Append($"{myKey}:");
        if (Keys.Count == 0) builder.AppendLine(" [Empty]");
        builder.AppendLine();
        foreach (var key in Keys) {
            builder.Append($"{indent}- ");
            BuildItem(builder, key, this[key], level);
        }
    }

    private static void BuildItem(StringBuilder builder, string key, object? value, uint level) {
        switch (value) {
            case null:
                builder.AppendLine($"{key}: null");
                return;
            case IContext context:
                context.BuildDescription(builder, key, level + 1);
                return;
            case string:
                builder.AppendLine($"{key}: {value}");
                return;
            case IEnumerable items:
                var counter = 1;
                builder.AppendLine($"{key}:");
                foreach (var item in items)
                    BuildItem(builder, $"{counter++} => ", item, level);
                return;
            case not null when value.GetType().IsClass:
                builder.AppendLine($"{key}: {JsonSerializer.Serialize(value)}");
                return;
            default:
                builder.AppendLine($"{key}: {value}");
                return;
        }
    }
}
