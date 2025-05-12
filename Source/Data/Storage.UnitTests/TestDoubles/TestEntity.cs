namespace DotNetToolbox.Data.TestDoubles;

internal sealed class TestEntity()
    : IEntity<uint> {
    public TestEntity(uint id, string name)
        : this() {
        Id = id;
        Name = name;
    }

    public TestEntity(string name)
        : this() {
        Name = name;
    }

    public uint Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public Result Validate(IMap? context = null) => Result.Success();
};
