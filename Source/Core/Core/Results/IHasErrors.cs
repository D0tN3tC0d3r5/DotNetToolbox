namespace DotNetToolbox.Results;

public interface IHasErrors {
    IReadOnlyList<Error> Errors { get; }
    bool HasErrors { get; }
}
