// ReSharper disable once CheckNamespace
namespace System.Text;

public static class StringBuilderExtensions {
    public static StringBuilder AppendRandomCharFrom(this StringBuilder builder, ReadOnlySpan<char> charSet)
        => builder.Append(charSet[Random.Shared.Next(charSet.Length)]);

    public static StringBuilder AppendRandomCharsFrom(this StringBuilder builder, uint count, ReadOnlySpan<char> charSet) {
        for (var i = 0; i < count; i++)
            builder.AppendRandomCharFrom(charSet);
        return builder;
    }

    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, StringBuilder? input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, StringBuilder? input, int startIndex, int length) {
        for (var i = 0; i < count; i++) builder.Append(input, startIndex, length);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, string? input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, string? input, int startIndex, int length) {
        for (var i = 0; i < count; i++) builder.Append(input, startIndex, length);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, char[]? input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, char[]? input, int startIndex, int length) {
        for (var i = 0; i < count; i++) builder.Append(input, startIndex, length);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, ReadOnlySpan<char> input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, ReadOnlyMemory<char> input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, bool input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, char input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, sbyte input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, byte input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, short input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, int input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, long input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, float input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, double input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, decimal input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, ushort input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, uint input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, ulong input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendTimes(this StringBuilder builder, uint count, object? input) {
        for (var i = 0; i < count; i++) builder.Append(input);
        return builder;
    }

    public static StringBuilder AppendLines(this StringBuilder builder, uint count) {
        for (var i = 0; i < count; i++) builder.AppendLine();
        return builder;
    }
    public static StringBuilder AppendLines(this StringBuilder builder, uint count, string? input) {
        for (var i = 0; i < count; i++) builder.AppendLine(input);
        return builder;
    }

    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, StringBuilder? input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, StringBuilder? input, int startIndex, int length) {
        while (predicate()) builder.Append(input, startIndex, length);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, string? input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, string? input, int startIndex, int length) {
        while (predicate()) builder.Append(input, startIndex, length);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, char[]? input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, char[]? input, int startIndex, int length) {
        while (predicate()) builder.Append(input, startIndex, length);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, ReadOnlySpan<char> input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, ReadOnlyMemory<char> input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, bool input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, char input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, sbyte input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, byte input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, short input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, int input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, long input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, float input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, double input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, decimal input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, ushort input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, uint input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, ulong input) {
        while (predicate()) builder.Append(input);
        return builder;
    }
    public static StringBuilder AppendWhile(this StringBuilder builder, Func<bool> predicate, object? input) {
        while (predicate()) builder.Append(input);
        return builder;
    }

    public static StringBuilder AppendIntoNewLine(this StringBuilder builder, string? text)
        => builder.AppendIntoNewLine(string.Empty, text);

    public static StringBuilder AppendIntoNewLine(this StringBuilder builder, string ident, string? text)
        => (string.IsNullOrWhiteSpace(text) || builder.Length == 0
               ? builder
               : builder.AppendLine())
           .Append($"{ident}{text}");
}
