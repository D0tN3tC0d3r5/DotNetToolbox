namespace DotNetToolbox.Security;

public interface IPasswordPolicy {
    Result Enforce(string password);
}