
namespace Sophia.WebApp.Data.World;

public interface IHasFacts<TKey> {
    TKey Id { get; set; }
    HashSet<string> Facts { get; set; }
}
