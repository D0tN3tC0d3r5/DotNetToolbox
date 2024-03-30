
namespace Sophia.WebApp.Data.World;

public interface IHasFacts<TKey> {
    TKey Id { get; set; }
    List<string> Facts { get; set; }
}
