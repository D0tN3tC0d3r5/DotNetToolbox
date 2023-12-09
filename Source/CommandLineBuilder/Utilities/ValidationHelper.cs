namespace DotNetToolbox.CommandLineBuilder.Utilities;

internal static class ValidationHelper {
    internal static void ValidateName(string name) {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"The {nameof(name)} cannot be null or whitespace.", nameof(name));
    }

    internal static void ValidateParameterIndex<T>(ICollection<T> parameters, uint index) {
        if (parameters.Count == 0)
            throw new ArgumentException("The command contains no parameters.", nameof(index));
        if (index >= parameters.Count)
            throw new ArgumentOutOfRangeException(nameof(index), index, $"The {nameof(index)} is out of range (Min: 0, Max: {parameters.Count - 1}).");
    }
}
