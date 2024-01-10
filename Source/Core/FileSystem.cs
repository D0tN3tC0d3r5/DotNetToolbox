namespace DotNetToolbox;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for OS functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for testing.
public class FileSystem {
    public static readonly FileSystem Default = new();

    public virtual char DirectorySeparatorChar => Path.DirectorySeparatorChar;
    public virtual string CombinePath(params string[] paths) => Path.Combine(paths);
    public virtual string[] GetPath(string filePath)
        => [.. Path.GetDirectoryName(filePath)?.Split(DirectorySeparatorChar) ?? [], Path.GetFileName(filePath)];
    public virtual string GetFileName(string filePath) => Path.GetFileName(filePath);
    public virtual string GetFileNameOnly(string filePath) => Path.GetFileNameWithoutExtension(filePath);
    public virtual string GetFileExtension(string filePath) => Path.GetExtension(filePath).TrimStart('.');

    public virtual string[] GetFilesFrom(string folderPath, string searchPattern, SearchOption searchOptions)
        => Directory.GetFiles(folderPath, searchPattern, searchOptions);
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
