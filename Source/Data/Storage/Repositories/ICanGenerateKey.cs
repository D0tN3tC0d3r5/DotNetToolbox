namespace DotNetToolbox.Data.Repositories;

public interface ICanGenerateKey {
    Task<object> GenerateKey(CancellationToken ct = default);
}
