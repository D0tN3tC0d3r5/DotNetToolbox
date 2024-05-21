namespace DotNetToolbox.ConsoleApplication.TestDoubles;

public sealed class TestFileProvider
    : IFileProvider {
    public IFileInfo GetFileInfo(string subpath) => new TestFileInfo();
    public IDirectoryContents GetDirectoryContents(string subpath) => throw new NotImplementedException();
    public IChangeToken Watch(string filter) => new TestChangeToken();
}
