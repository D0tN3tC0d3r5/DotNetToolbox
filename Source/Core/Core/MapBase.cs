namespace DotNetToolbox;

public abstract class MapBase {
    protected abstract void ToText(StringBuilder builder, string? name = null, uint level = 0);

    protected static void BuildItem(StringBuilder builder, string key, object? value, uint level) {
        switch (value) {
            case null:
                builder.AppendLine($"{key}: null");
                return;
            case MapBase contextBase:
                contextBase.ToText(builder, key, level + 1);
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
