namespace DotNetToolbox.ConsoleApplication.TestDoubles;

internal sealed class TestFileSystemAccessor : IFileSystemAccessor {
    public string CombinePath(params string[] paths) => throw new NotImplementedException();

    public void CopyFile(string sourcePath, string targetPath, bool overwrite = false) => throw new NotImplementedException();

    public void CreateFolder(string folderPath) => throw new NotImplementedException();

    public Stream CreateNewOrOverwriteFile(string filePath, bool blockExternalAccess = true) => throw new NotImplementedException();

    public void DeleteFile(string sourcePath) => throw new NotImplementedException();

    public void DeleteFolder(string folderPath, bool includeAllContent = false) => throw new NotImplementedException();

    public bool FileExists(string filePath) => throw new NotImplementedException();
    public string GetFileExtension(string filePath, bool includeDot = true) => throw new NotImplementedException();
    public string GetFileName(string filePath, bool includeExtension = false) => throw new NotImplementedException();
    public IEnumerable<string> GetEntries(string baseFolder) => throw new NotImplementedException();
    public IEnumerable<string> GetEntries(string baseFolder, string searchPattern) => throw new NotImplementedException();
    public IEnumerable<string> GetEntries(string baseFolder, EnumerationOptions enumerationOptions) => throw new NotImplementedException();
    public IEnumerable<string> GetEntries(string baseFolder, string searchPattern, EnumerationOptions enumerationOptions) => throw new NotImplementedException();
    public IEnumerable<string> GetFiles(string baseFolder) => throw new NotImplementedException();
    public IEnumerable<string> GetFiles(string baseFolder, string searchPattern) => throw new NotImplementedException();
    public IEnumerable<string> GetFiles(string baseFolder, EnumerationOptions enumerationOptions) => throw new NotImplementedException();
    public IEnumerable<string> GetFiles(string baseFolder, string searchPattern, EnumerationOptions enumerationOptions) => throw new NotImplementedException();
    public bool FolderExists(string folderPath) => throw new NotImplementedException();
    public string CurrentFolder {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public IEnumerable<string> GetFolders(string baseFolder) => throw new NotImplementedException();
    public IEnumerable<string> GetFolders(string baseFolder, string searchPattern) => throw new NotImplementedException();
    public IEnumerable<string> GetFolders(string baseFolder, EnumerationOptions enumerationOptions) => throw new NotImplementedException();
    public IEnumerable<string> GetFolders(string baseFolder, string searchPattern, EnumerationOptions enumerationOptions) => throw new NotImplementedException();
    public string[] GetFilePath(string filePath) => throw new NotImplementedException();
    public void MoveFile(string sourcePath, string targetPath, bool overwrite = false) => throw new NotImplementedException();
    public Stream OpenFileAsReadOnly(string filePath, bool blockExternalAccess = true) => throw new NotImplementedException();
    public Stream OpenOrCreateFile(string filePath, bool blockExternalAccess = true) => throw new NotImplementedException();
    public char DirectorySeparatorChar => throw new NotImplementedException();

    public char FolderSeparator => throw new NotImplementedException();
}
