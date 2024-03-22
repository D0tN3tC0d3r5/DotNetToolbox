
namespace Sophia.WebApp.Data.World;

public interface IHasFacts<TKey> {
    TKey Id { get; set; }
    List<FactEntity> Facts { get; set; }
}
