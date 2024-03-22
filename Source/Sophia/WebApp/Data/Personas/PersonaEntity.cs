namespace Sophia.WebApp.Data.Personas;

[Table("Personas")]
[EntityTypeConfiguration(typeof(PersonaEntity))]
public class PersonaEntity
    : IEntityTypeConfiguration<PersonaEntity>
    , IHasTools<int>
    , IHasFacts<int> {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = "Agent";
    [MaxLength(1000)]
    public string Description { get; set; } = "You are a helpful agent.";
    [MaxLength(1000)]
    public string? Personality { get; set; }
    public string[] Instructions { get; set; } = [];
    public List<FactEntity> Facts { get; set; } = [];
    public List<ToolEntity> Tools { get; set; } = [];

    public void Configure(EntityTypeBuilder<PersonaEntity> builder) {
        builder.HasKey(p => p.Id);
        builder.PrimitiveCollection(p => p.Instructions);
        builder.HasMany(w => w.Facts)
               .WithMany()
               .UsingEntity<PersonaFactsEntity>(l => l.HasOne<FactEntity>()
                                                      .WithMany()
                                                      .HasForeignKey(e => e.FactId),
                                                r => r.HasOne<PersonaEntity>()
                                                      .WithMany()
                                                      .HasForeignKey(e => e.PersonaId));
        builder.HasMany(p => p.Tools)
               .WithMany()
               .UsingEntity<PersonaToolsEntity>(l => l.HasOne<ToolEntity>()
                                                      .WithMany()
                                                      .HasForeignKey(e => e.ToolId),
                                                r => r.HasOne<PersonaEntity>()
                                                      .WithMany()
                                                      .HasForeignKey(e => e.PersonaId));
    }

    public static async Task Seed(ApplicationDbContext dbContext) {
        if (await dbContext.Personas.AnyAsync()) return;
        var persona = new PersonaEntity();
        dbContext.Personas.Add(persona);
    }

    public PersonaData ToDto()
        => new() {
            Id = Id,
            Name = Name,
            Description = Description,
            Personality = Personality,
            Instructions = [.. Instructions],
            Facts = Facts.ToList(f => f.ToDto()),
            KnownTools = Tools.ToList(f => f.ToDto()),
        };
}
