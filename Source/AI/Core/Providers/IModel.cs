namespace DotNetToolbox.AI.Providers;

public interface IModel {
    string Id { get; set; }
    string Name { get; set; }
    string Provider { get; set; }
}
