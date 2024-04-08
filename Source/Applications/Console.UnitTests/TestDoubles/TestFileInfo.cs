namespace DotNetToolbox.ConsoleApplication.TestDoubles;

public sealed class TestFileInfo
    : IFileInfo {
    private const string _contents = "{}";
    public Stream CreateReadStream() => new MemoryStream(Encoding.UTF8.GetBytes(_contents));
    public bool Exists => true;
    public long Length => 100;
    public string? PhysicalPath => null;
    public string Name => "settings.json";
    public DateTimeOffset LastModified => DateTimeOffset.Now.AddDays(-1);
    public bool IsDirectory => false;
}
