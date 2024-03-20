namespace Sophia.WebApp.Data.Personas;

[Table("PersonaFacts")]
public class PersonaFactsEntity {
    public int PersonaId { get; set; }
    public int FactId { get; set; }
}

[Table("PersonaTools")]
public class PersonaToolsEntity {
    public int PersonaId { get; set; }
    public int ToolId { get; set; }
}

[Table("Personas")]
[EntityTypeConfiguration(typeof(PersonaEntity))]
public class PersonaEntity
    : IEntityTypeConfiguration<PersonaEntity> {
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
    public List<ToolEntity> KnownTools { get; set; } = [];

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
        builder.HasMany(p => p.KnownTools)
               .WithMany()
               .UsingEntity<PersonaToolsEntity>(l => l.HasOne<ToolEntity>()
                                                      .WithMany()
                                                      .HasForeignKey(e => e.ToolId),
                                                r => r.HasOne<PersonaEntity>()
                                                      .WithMany()
                                                      .HasForeignKey(e => e.PersonaId));
    }

    public PersonaData ToDto()
        => new() {
            Id = Id,
            Name = Name,
            Description = Description,
            Personality = Personality,
            Instructions = [.. Instructions],
            Facts = Facts.ToList(f => f.ToDto()),
            KnownTools = KnownTools.ToList(f => f.ToDto()),
        };
}
