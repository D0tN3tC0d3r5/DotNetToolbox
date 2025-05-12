namespace DotNetToolbox.Data.KeyGenerators;

public interface IKeyGenerator<out TKey> {
    TKey GenerateNextKey(string? storageKey = null);
    Result Initialize(string? storageKey = null);
}
