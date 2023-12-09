namespace DotNetToolbox.CommandLineBuilder.TestUtilities;

public class InMemoryOutputWriter : OutputWriter {
    public string Output = string.Empty;

    public override void ResetColor() { }

    public override void Write(bool value) => Output += $"{value}";
    public override void Write(uint value) => Output += $"{value}";
    public override void Write(ulong value) => Output += $"{value}";
    public override void Write(int value) => Output += $"{value}";
    public override void Write(long value) => Output += $"{value}";
    public override void Write(float value) => Output += $"{value}";
    public override void Write(double value) => Output += $"{value}";
    public override void Write(decimal value) => Output += $"{value}";
    public override void Write(char value) => Output += $"{value}";
    public override void Write(string? value) => Output += $"{value}";
    public override void Write(object? value) => Output += $"{value}";
    public override void Write(StringBuilder? builder) => Output += builder?.ToString() ?? string.Empty;
    public override void Write(string format, object? arg0) => Output += string.Format(format, arg0);
    public override void Write(string format, object? arg0, object? arg1) => Output += string.Format(format, arg0, arg1);
    public override void Write(string format, object? arg0, object? arg1, object? arg2) => Output += string.Format(format, arg0, arg1, arg2);
    public override void Write(string format, params object?[] args) => Output += string.Format(format, args);
    public override void Write(char[]? buffer) => Output += new string(buffer);
    public override void Write(char[] buffer, int index, int count) => Output += new string(buffer, index, count);
    public override void WriteLine() => Output += "\n";
    public override void WriteLine(bool value) => Output += $"{value}\n";
    public override void WriteLine(uint value) => Output += $"{value}\n";
    public override void WriteLine(ulong value) => Output += $"{value}\n";
    public override void WriteLine(int value) => Output += $"{value}\n";
    public override void WriteLine(long value) => Output += $"{value}\n";
    public override void WriteLine(float value) => Output += $"{value}\n";
    public override void WriteLine(double value) => Output += $"{value}\n";
    public override void WriteLine(decimal value) => Output += $"{value}\n";
    public override void WriteLine(char value) => Output += $"{value}\n";
    public override void WriteLine(string? value) => Output += $"{value}\n";
    public override void WriteLine(object? value) => Output += $"{value}\n";
    public override void WriteLine(StringBuilder? builder) => Output += builder?.AppendLine().ToString() ?? "\n";
    public override void WriteLine(string format, object? arg0) => Output += string.Format(format, arg0) + "\n";
    public override void WriteLine(string format, object? arg0, object? arg1) => Output += string.Format(format, arg0, arg1) + "\n";
    public override void WriteLine(string format, object? arg0, object? arg1, object? arg2) => Output += string.Format(format, arg0, arg1, arg2) + "\n";
    public override void WriteLine(string format, params object?[] args) => Output += string.Format(format, args) + "\n";
    public override void WriteLine(char[]? buffer) => Output += new string(buffer) + "\n";
    public override void WriteLine(char[] buffer, int index, int count) => Output += new string(buffer, index, count) + "\n";
}
