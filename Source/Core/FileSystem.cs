namespace DotNetToolbox;

public interface IFileSystem {
    char DirectorySeparatorChar { get; }

    string CombinePath(params string[] paths);
    void CopyFile(string sourcePath, string targetPath, bool overwrite = false);
    void CreateFolder(string folderPath);
    Stream CreateNewOrOverwriteFile(string filePath, bool blockExternalAccess = true);
    void DeleteFile(string sourcePath);
    void DeleteFolder(string folderPath, bool includeAllContent = false);
    bool FileExists(string filePath);
    string[] GetFilePath(string filePath);
    string GetFileName(string filePath, bool includeExtension = false);
    string GetFileExtension(string filePath, bool includeDot = true);
    char FolderSeparator { get; }
    string CurrentFolder { get; set; }
    bool FolderExists(string folderPath);
    IEnumerable<string> GetEntries(string baseFolder);
    IEnumerable<string> GetEntries(string baseFolder, string searchPattern);
    IEnumerable<string> GetEntries(string baseFolder, EnumerationOptions enumerationOptions);
    IEnumerable<string> GetEntries(string baseFolder, string searchPattern, EnumerationOptions enumerationOptions);
    IEnumerable<string> GetFiles(string baseFolder);
    IEnumerable<string> GetFiles(string baseFolder, string searchPattern);
    IEnumerable<string> GetFiles(string baseFolder, EnumerationOptions enumerationOptions);
    IEnumerable<string> GetFiles(string baseFolder, string searchPattern, EnumerationOptions enumerationOptions);
    IEnumerable<string> GetFolders(string baseFolder);
    IEnumerable<string> GetFolders(string baseFolder, string searchPattern);
    IEnumerable<string> GetFolders(string baseFolder, EnumerationOptions enumerationOptions);
    IEnumerable<string> GetFolders(string baseFolder, string searchPattern, EnumerationOptions enumerationOptions);
    void MoveFile(string sourcePath, string targetPath, bool overwrite = false);
    Stream OpenFileAsReadOnly(string filePath, bool blockExternalAccess = true);
    Stream OpenOrCreateFile(string filePath, bool blockExternalAccess = true);
}

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for OS functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for externally.
public class FileSystem : HasDefault<FileSystem>, IFileSystem {
    public virtual char DirectorySeparatorChar => Path.DirectorySeparatorChar;
    public virtual string CombinePath(params string[] paths) => Path.Combine(paths);
    public virtual string[] GetFilePath(string filePath)
        => [.. Path.GetDirectoryName(filePath)?.Split(DirectorySeparatorChar) ?? [], GetFileName(filePath, true)];
    public virtual string GetFileName(string filePath, bool includeExtension = false)
        => includeExtension
               ? Path.GetFileName(filePath)
               : Path.GetFileNameWithoutExtension(filePath);
    public virtual string GetFileExtension(string filePath, bool includeDot = true)
        => includeDot
               ? Path.GetExtension(filePath)
               : Path.GetExtension(filePath).TrimStart('.');

    public virtual string CurrentFolder {
        get => Directory.GetCurrentDirectory();
        set => Directory.SetCurrentDirectory(value);
    }

    public virtual bool FolderExists(string folderPath)
        => Directory.Exists(folderPath);
    public virtual IEnumerable<string> GetEntries(string baseFolder)
        => Directory.EnumerateFileSystemEntries(baseFolder);
    public virtual IEnumerable<string> GetEntries(string baseFolder, string searchPattern)
        => Directory.EnumerateFileSystemEntries(baseFolder, searchPattern);
    public virtual IEnumerable<string> GetEntries(string baseFolder, EnumerationOptions enumerationOptions)
        => Directory.EnumerateFileSystemEntries(baseFolder, "*", enumerationOptions);
    public virtual IEnumerable<string> GetEntries(string baseFolder, string searchPattern, EnumerationOptions enumerationOptions)
        => Directory.EnumerateFileSystemEntries(baseFolder, searchPattern, enumerationOptions);
    public virtual IEnumerable<string> GetFiles(string baseFolder)
        => Directory.EnumerateFiles(baseFolder);
    public virtual IEnumerable<string> GetFiles(string baseFolder, string searchPattern)
        => Directory.EnumerateFiles(baseFolder, searchPattern);
    public virtual IEnumerable<string> GetFiles(string baseFolder, EnumerationOptions enumerationOptions)
        => Directory.EnumerateFiles(baseFolder, "*", enumerationOptions);
    public virtual IEnumerable<string> GetFiles(string baseFolder, string searchPattern, EnumerationOptions enumerationOptions)
        => Directory.EnumerateFiles(baseFolder, searchPattern, enumerationOptions);
    public virtual IEnumerable<string> GetFolders(string baseFolder)
        => Directory.EnumerateDirectories(baseFolder);
    public virtual IEnumerable<string> GetFolders(string baseFolder, string searchPattern)
        => Directory.EnumerateDirectories(baseFolder, searchPattern);
    public virtual IEnumerable<string> GetFolders(string baseFolder, EnumerationOptions enumerationOptions)
        => Directory.EnumerateDirectories(baseFolder, "*", enumerationOptions);
    public virtual IEnumerable<string> GetFolders(string baseFolder, string searchPattern, EnumerationOptions enumerationOptions)
        => Directory.EnumerateDirectories(baseFolder, searchPattern, enumerationOptions);

    public virtual char FolderSeparator => Path.DirectorySeparatorChar;
    public virtual void CreateFolder(string folderPath)
        => Directory.CreateDirectory(folderPath);
    public virtual void DeleteFolder(string folderPath, bool includeAllContent = false)
        => Directory.Delete(folderPath, includeAllContent);

    public virtual bool FileExists(string filePath) => File.Exists(filePath);
    public virtual void MoveFile(string sourcePath, string targetPath, bool overwrite = false) => File.Move(sourcePath, targetPath, overwrite);
    public virtual void CopyFile(string sourcePath, string targetPath, bool overwrite = false) => File.Copy(sourcePath, targetPath, overwrite);
    public virtual void DeleteFile(string sourcePath) => File.Delete(sourcePath);

    public virtual Stream OpenFileAsReadOnly(string filePath, bool blockExternalAccess = true) => new FileStream(filePath, FileMode.Open, FileAccess.Read, blockExternalAccess ? FileShare.None : FileShare.ReadWrite);
    public virtual Stream OpenOrCreateFile(string filePath, bool blockExternalAccess = true) => new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, blockExternalAccess ? FileShare.None : FileShare.Read);
    public virtual Stream CreateNewOrOverwriteFile(string filePath, bool blockExternalAccess = true) => new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, blockExternalAccess ? FileShare.None : FileShare.Read);
}
