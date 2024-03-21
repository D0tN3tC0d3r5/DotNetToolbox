namespace Sophia.WebApp.Data.World;

[Table("WorldFacts")]
public class WorldFactsEntity {
    public Guid WorldId { get; set; }
    public int FactId { get; set; }
}