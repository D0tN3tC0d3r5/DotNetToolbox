namespace System.Security;

public interface IPasswordPolicy {
    Result Enforce(string password);
}