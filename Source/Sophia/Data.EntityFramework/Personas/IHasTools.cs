namespace Sophia.Data.Personas;

public interface IHasTools<TKey> {
    TKey Id { get; set; }
    List<ToolEntity> Tools { get; set; }
}
