
namespace Sophia.Data.World;

public interface IHasTools<TKey> {
    TKey Id { get; set; }
    List<ToolEntity> Tools { get; set; }
}
