namespace WebApi.Options;

public record CacheOptions {
    public CacheType Type { get; set; }
    public string ConnectionString { get; set; } = string.Empty;
    public string DistributedCacheType { get; set; } = string.Empty;

    public virtual Result Validate(IMap? context = null)
        => Result.Default;
}

public record CorsOptions {
    public bool Disabled { get; set; }
    public virtual Result Validate(IMap? context = null)
        => Result.Default;
}