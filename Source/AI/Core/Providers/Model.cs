namespace DotNetToolbox.AI.Providers;

public class Model
    : IModel {
    public string Provider { get; set; } = default!;
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}
