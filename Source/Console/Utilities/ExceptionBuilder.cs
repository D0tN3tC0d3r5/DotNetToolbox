namespace DotNetToolbox.ConsoleApplication.Utilities;

internal class ExceptionBuilder(Exception exception) {
    public static string Build(Exception exception) {
        var builder = new ExceptionBuilder(exception);
        return builder.Build();
    }

    public string Build() {
        var builder = new StringBuilder();
        ShowException(builder, exception);
        return builder.ToString();
    }

    private void ShowException(StringBuilder builder, Exception ex, bool isInner = false, int indent = 0) {
        ShowDescription(builder, ex, isInner, indent);
        ShowStackTrace(builder, ex, indent + 1);
        if (ex.InnerException is not null) ShowException(builder, ex.InnerException, true, indent + 1);
    }

    private void ShowDescription(StringBuilder builder, Exception ex, bool isInner, int indent) {
        builder.Append(' ', indent * 4);
        if (isInner) builder.Append("Inner Exception => ");
        builder.Append(ex.GetType().Name);
        builder.Append(": ");
        builder.AppendLine(ex.Message);
    }

    private void ShowStackTrace(StringBuilder builder, Exception ex, int indent) {
        builder.Append(' ', indent * 4).AppendLine("Stack Trace:");
        var lines = ex.StackTrace?.Split(Environment.NewLine) ?? [];
        _ = lines.Aggregate(builder, (s, l) => s.Append(' ', (indent + 1) * 4).AppendLine(l));
    }
}
