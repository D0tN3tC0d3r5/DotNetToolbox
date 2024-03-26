namespace Sophia.WebApp.Data.Providers;

[Table("Providers")]
[EntityTypeConfiguration(typeof(ProviderEntity))]
public class ProviderEntity
    : IEntityTypeConfiguration<ProviderEntity>,
      IHasModels {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public ApiEntity Api { get; set; } = new();
    public AuthenticationEntity Authentication { get; set; } = new();
    public List<ModelEntity> Models { get; set; } = [];

    public void Configure(EntityTypeBuilder<ProviderEntity> builder) {
        builder.HasKey(p => p.Id);
        builder.HasMany(p => p.Models)
               .WithOne(m => m.Provider)
               .HasForeignKey(m => m.ProviderId);
        builder.ComplexProperty(p => p.Api);
        builder.ComplexProperty(p => p.Authentication);
    }

    public static async Task Seed(ApplicationDbContext dbContext) {
        if (await dbContext.Providers.AnyAsync()) return;
        var openAi = new ProviderEntity {
            Name = "OpenAI",
            Api = new() {
                BaseAddress = "https://api.openai.com",
                ChatEndpoint = "v1/completions",
            },
            Authentication = new() {
                Type = AuthenticationType.StaticToken,
            },
            Models = [
                new() {
                    Key = "gpt-4-turbo-preview",
                    Name = "GPT 4 Turbo",
                },
                new() {
                    Key = "gpt-3.5-turbo",
                    Name = "GPT 3.5 Turbo",
                },
            ],
        };
        var anthropic = new ProviderEntity {
            Name = "Anthropic",
            Api = new() {
                BaseAddress = "https://api.anthropic.com",
                ChatEndpoint = "v1/messages",
            },
            Authentication = new() {
                Type = AuthenticationType.ApiKey,
            },
            Models = [
                new() {
                    Key = "claude-3-opus-20240229",
                    Name = "Claude 3 Opus",
                },
                new() {
                    Key = "claude-3-haiku-20240307",
                    Name = "Claude 3 Haiku",
                },
                new() {
                    Key = "claude-2.1",
                    Name = "Claude 2.1",
                },
            ],
        };
        dbContext.Providers.AddRange(openAi, anthropic);
    }

    public ProviderData ToDto()
        => new() {
            Id = Id,
            Name = Name,
            Api = Api.ToDto(),
            Authentication = Authentication.ToDto(),
            Models = Models.ToList(f => f.ToDto()),
        };
}
