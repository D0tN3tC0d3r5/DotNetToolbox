namespace System.Pagination;

public static class BlockSettings {
    public const uint MaxSize = 1_000;
    public const uint MinSize = 1;
    public const uint MaxCount = 1_000_000_000;
    public const uint MaxIndex = 999_999_999;

    public const uint DefaultBlockSize = 20;
}
