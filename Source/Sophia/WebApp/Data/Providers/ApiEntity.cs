namespace Sophia.WebApp.Data.Providers;

[EntityTypeConfiguration(typeof(ApiEntity))]
public class ApiEntity
    : IEntityTypeConfiguration<ApiEntity> {
    [Required]
    [MaxLength(1000)]
    public string BaseAddress { get; init; } = string.Empty;
    [Required]
    [MaxLength(1000)]
    public string ChatEndpoint { get; init; } = string.Empty;
    [Required]
    public AuthorizationType AuthorizationType { get; init; }
    [Required]
    public AuthorizationScheme AuthorizationScheme { get; init; } = AuthorizationScheme.Basic;
    [MaxLength(4000)]
    public string? AuthorizationValue { get; set; }
    public DateTimeOffset? AuthorizationExpiresOn { get; set; }
    public List<string> CustomRequestHeaders { get; set; } = [];

    public void Configure(EntityTypeBuilder<ApiEntity> builder) {
        builder.HasNoKey();
        builder.PrimitiveCollection(p => p.CustomRequestHeaders);
    }

    public ApiData ToDto()
        => new() {
            BaseAddress = BaseAddress,
            ChatEndpoint = ChatEndpoint,
            Authorization = new() {
                Type = AuthorizationType,
                Scheme = AuthorizationScheme,
                Value = AuthorizationValue,
                ExpiresOn = AuthorizationExpiresOn,
            },
            CustomRequestHeaders = CustomRequestHeaders.Select(i => i.Split('|'))
                                                       .ToDictionary(k => k[0], v => new[] { v[1] }),
        };
}
