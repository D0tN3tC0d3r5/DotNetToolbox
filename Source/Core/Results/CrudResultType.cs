namespace DotNetToolbox.Results;

public enum CrudResultType {
    Error = -1,
    Invalid = 0,
    Success = 1,
    NotFound = 2,
    Conflict = 3,
}
