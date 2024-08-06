namespace DotNetToolbox.ValidationBuilder;

[Flags]
public enum ValidatorMode {
    Also = 0b0000,
    Not = 0b0001,
    And = 0b0010,
    AndNot = And | Not,
    Or = 0b0100,
    OrNot = Or | Not,
}

public static class ValidatorModeExtensions {
    public static bool Has(this ValidatorMode mode, ValidatorMode flag)
        => mode.HasFlag(flag);
}
