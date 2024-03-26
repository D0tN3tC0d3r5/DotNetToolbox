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
    public List<string> CustomRequestHeaders { get; set; } = [];

    public void Configure(EntityTypeBuilder<ApiEntity> builder) {
        builder.HasNoKey();
        builder.PrimitiveCollection(p => p.CustomRequestHeaders);
    }

    public ApiData ToDto()
        => new() {
            BaseAddress = BaseAddress,
            ChatEndpoint = ChatEndpoint,
            CustomRequestHeaders = CustomRequestHeaders.Select(i => i.Split('|'))
                                                       .ToDictionary(k => k[0], v => v[1]),
        };
}
