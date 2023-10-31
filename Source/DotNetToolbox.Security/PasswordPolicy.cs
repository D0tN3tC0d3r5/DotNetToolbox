using System.Security;

namespace DotNetToolbox.Security;

public class PasswordPolicy : IPasswordPolicy {
    public virtual Result Enforce(string password) => Result.Success();
}