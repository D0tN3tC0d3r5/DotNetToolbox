namespace DotNetToolbox.ConsoleApplication.TestDoubles;

internal class TestFileSystem() : IFileSystem {
    public string CombinePath(params string[] paths) => throw new NotImplementedException();

    public void CopyFile(string sourcePath, string targetPath, bool overwrite = false) => throw new NotImplementedException();

    public void CreateFolder(string folderPath) => throw new NotImplementedException();

    public Stream CreateNewOrOverwriteFile(string filePath, bool blockExternalAccess = true) => throw new NotImplementedException();

    public void DeleteFile(string sourcePath) => throw new NotImplementedException();

    public void DeleteFolder(string folderPath, bool includeAllContent = false) => throw new NotImplementedException();

    public bool FileExists(string filePath) => throw new NotImplementedException();

    public string GetFileExtension(string filePath) => throw new NotImplementedException();

    public string GetFileName(string filePath) => throw new NotImplementedException();

    public string GetFileNameOnly(string filePath) => throw new NotImplementedException();

    public string[] GetFilesFrom(string folderPath, string searchPattern, SearchOption searchOptions) => throw new NotImplementedException();

    public string[] GetPath(string filePath) => throw new NotImplementedException();

    public void MoveFile(string sourcePath, string targetPath, bool overwrite = false) => throw new NotImplementedException();

    public Stream OpenFileAsReadOnly(string filePath, bool blockExternalAccess = true) => throw new NotImplementedException();

    public Stream OpenOrCreateFile(string filePath, bool blockExternalAccess = true) => throw new NotImplementedException();

    public char DirectorySeparatorChar => throw new NotImplementedException();
}
