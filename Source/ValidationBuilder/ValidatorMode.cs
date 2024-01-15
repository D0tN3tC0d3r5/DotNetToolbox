namespace DotNetToolbox.ValidationBuilder;

[Flags]
[SuppressMessage("Roslynator", "RCS1135:Declare enum member with zero value (when enum has FlagsAttribute)", Justification = "<Pending>")]
public enum ValidatorMode {
    And =        0b0001,
    Or =         0b0010,
    Not =        0b0100,
    AndNot =  And | Not,
    OrNot =    Or | Not,
}

public static class ValidatorModeExtensions {
    public static bool Has(this ValidatorMode mode, ValidatorMode flag)
        => (mode & flag) == flag;
}
