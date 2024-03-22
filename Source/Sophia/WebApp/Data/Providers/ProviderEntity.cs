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
    [MaxLength(2000)]
    public string? Authentication { get; set; }
    public List<ModelEntity> Models { get; set; } = [];

    public void Configure(EntityTypeBuilder<ProviderEntity> builder) {
        builder.HasKey(p => p.Id);
        builder.HasMany(p => p.Models)
               .WithOne(m => m.Provider)
               .HasForeignKey(m => m.ProviderId);
    }

    public static async Task Seed(ApplicationDbContext dbContext) {
        if (await dbContext.Providers.AnyAsync()) return;
        var openAi = new ProviderEntity {
            Name = "OpenAI",
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
            Models = [
                new() {
                    Key = "claude-3-opus-20240229",
                    Name = "Claude 3 Opus",
                },
                new() {
                    Key = "claude-3-haiku-20240307",
                    Name = "Claude 3 Haiku",
                },
            ],
        };
        dbContext.Providers.AddRange(openAi, anthropic);
    }

    public ProviderData ToDto()
        => new() {
            Id = Id,
            Name = Name,
            Authentication = Authentication,
            Models = Models.ToList(f => f.ToDto()),
        };
}
