
namespace DotNetToolbox.ConsoleApplication.Application;

public interface IConfigurationBuilder {
    void AddAppSettings(IFileProvider? fileProvider = null);
    void AddEnvironmentVariables(string? prefix = null);
    void AddUserSecrets<TReference>() where TReference : class;
    void AddInMemoryValues(IDictionary<string, object?> values);
    IConfigurationRoot Build();
}
