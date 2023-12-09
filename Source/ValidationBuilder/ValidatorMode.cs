namespace DotNetToolbox.ValidationBuilder;

[Flags]
public enum ValidatorMode {
    Not = 0b0001,
    And = 0b0010,
    Or = 0b0100,
    AndNot = And | Not,
    OrNot = Or | Not,
}

public static class ValidatorModeExtensions {
    public static bool Has(this ValidatorMode mode, ValidatorMode flag)
        => (mode & flag) == flag;
}