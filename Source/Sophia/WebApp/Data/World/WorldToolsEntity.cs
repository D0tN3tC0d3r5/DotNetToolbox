namespace Sophia.WebApp.Data.World;

[Table("WorldTools")]
public class WorldToolsEntity {
    public Guid WorldId { get; set; }
    public int ToolId { get; set; }
}