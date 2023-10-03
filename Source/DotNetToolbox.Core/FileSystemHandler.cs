namespace System;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for OS functionality.")]
public class FileSystemHandler {
    public virtual string CombinePath(params string[] paths)
        => Path.Combine(paths);

    public virtual string GetFileNameFrom(string filePath)
        => Path.GetFileName(filePath);

    public virtual string[] GetFilesFrom(string folderPath, string searchPattern, SearchOption searchOptions)
        => Directory.GetFiles(folderPath, searchPattern, searchOptions);

    public virtual void CreateFolderIfNotExists(string folderPath)
        => Directory.CreateDirectory(folderPath);

    public virtual bool FileExists(string filePath)
        => File.Exists(filePath);

    public virtual void MoveFile(string sourcePath, string targetPath)
        => File.Move(sourcePath, targetPath);

    public virtual Stream OpenFileForReading(string filePath)
        => new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

    public virtual Stream CreateNewFileAndOpenForWriting(string filePath)
        => new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
}
