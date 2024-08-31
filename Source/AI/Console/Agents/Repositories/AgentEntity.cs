namespace AI.Sample.Agents.Repositories;

public class AgentEntity
        : Entity<AgentEntity, uint> {
    public string Name { get; set; } = string.Empty;

    public override Result Validate(IContext? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Name)) result += new ValidationError("Name is required.", nameof(Name));
        return result;
    }
}
