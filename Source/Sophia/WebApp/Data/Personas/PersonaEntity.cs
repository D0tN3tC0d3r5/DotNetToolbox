using static Sophia.WebApp.Data.Helpers.StringArrayConversion;

namespace Sophia.WebApp.Data.Personas;

public class PersonaEntity {
    [Key]
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

    public void Configure(EntityTypeBuilder<PersonaEntity> builder)
        => builder.Property(p => p.Instructions)
                  .HasConversion(o => ConvertToString(o), s => ConvertToArray(s), new StringArrayComparer());

    public PersonaData ToDto()
        => new() {
                     Id = Id,
                     Name = Name,
                     Description = Description,
                     Personality = Personality,
                     Instructions = [..Instructions],
                     Facts = Facts.ToList(f => f.ToDto()),
                     KnownTools = KnownTools.ToList(f => f.ToDto()),
                 };
}
