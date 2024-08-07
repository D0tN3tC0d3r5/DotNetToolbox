
namespace DotNetToolbox.AI.Jobs;

public interface IWorld {
    DateTimeOffset DateTime { get; }

    Result Validate(IDictionary<string, object?>? context = null);
}