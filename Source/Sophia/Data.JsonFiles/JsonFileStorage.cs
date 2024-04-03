namespace Sophia.Data;

public class JsonFileStorage<TData, TKey> : IJsonFileStorage<TData, TKey>
    where TData : class, ISimpleKeyEntity<TData, TKey>, new() {
    private readonly ILogger _logger;
    private readonly IFileSystemAccessor _io;
    private readonly IDateTimeProvider _dateTime;
    private readonly IGuidProvider _guidProvider;

    private string _repositoryPath;

    private const string _baseFolderKey = "JsonStorage:BaseFolder";

    public JsonFileStorage(IConfiguration configuration,
                           IFileSystemAccessor? io = null,
                           IDateTimeProvider? dateTime = null,
                           IGuidProvider? guidProvider = null,
                           ILoggerFactory? loggerFactory = null) {
        _io = io ?? new FileSystemAccessor();
        _guidProvider = guidProvider ?? new GuidProvider();
        _dateTime = dateTime ?? new DateTimeProvider();
        _logger = loggerFactory?.CreateLogger(GetType().Name) ?? NullLogger.Instance;
        _repositoryPath = IsNotNullOrWhiteSpace(configuration[_baseFolderKey], $"{nameof(configuration)}[{_baseFolderKey}]").Trim();
        _io.CreateFolder(_repositoryPath);
    }

    public void SetBasePath(string path) {
        _repositoryPath = _io.CombinePath(_repositoryPath, path);
        _io.CreateFolder(_repositoryPath);
    }

    public async Task<IEnumerable<TData>> GetAllAsync(Func<TData, bool>? filter = null, CancellationToken ct = default) {
        try {
            _logger.LogDebug("Getting files from '{FilePath}'...", _repositoryPath);
            var files = GetActiveFiles();
            var data = new List<TData>();
            foreach (var file in files) {
                var content = await GetFileDataOrDefaultAsync(file, ct).ConfigureAwait(false);
                if (content is null)
                    continue;
                data.Add(content);
            }

            if (filter is not null)
                data = data.Where(filter).ToList();

            _logger.LogDebug("{FileCount} files retrieved from '{FilePath}'.", data.Count, _repositoryPath);
            return [.. data];
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to get files from '{FilePath}'!", _repositoryPath);
            throw;
        }
    }

    public async Task<TData?> GetByIdAsync(TKey id, CancellationToken ct = default) {
        try {
            _logger.LogDebug("Getting latest data from '{FilePath}/{Id}'...", _repositoryPath, id);
            var file = GetActiveFile(id);
            if (file is null) {
                _logger.LogDebug("File '{FilePath}/{Id}' not found.", _repositoryPath, id);
                return default;
            }

            return await GetFileDataOrDefaultAsync(file, ct).ConfigureAwait(false);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to get data from file '{FilePath}/{Id}'!", _repositoryPath, id);
            throw;
        }
    }

    private readonly ConcurrentDictionary<string, long> _cachedKeys = [];
    private TKey GenerateNewKey()
        => (TKey)(object)(typeof(TKey).Name switch {
            nameof(Guid) => _guidProvider.Create(),
            nameof(Int32) => _cachedKeys.AddOrUpdate(_repositoryPath, 1, (_, value) => value + 1),
            nameof(Int64) => _cachedKeys.AddOrUpdate(_repositoryPath, 1, (_, value) => value + 1),
            nameof(DateTimeOffset) => _dateTime.UtcNow,
            nameof(DateTime) => _dateTime.UtcNow.DateTime,
            _ => throw new ArgumentException(nameof(ISimpleKeyEntity<TData, TKey>.Id), $"The id of type {typeof(TKey)} cannot be auto-generated."),
        });

    public async Task CreateAsync(TData data, CancellationToken ct = default) {
        try {
            if (data.Id?.Equals(default(TKey)) ?? true)
                data.Id = GenerateNewKey();
            _logger.LogDebug("Adding data in '{FilePath}/{Id}'...", _repositoryPath, data.Id);
            var filePath = GetActiveFile(data.Id);
            if (filePath is not null) {
                _logger.LogDebug("File '{FilePath}/{Id}' already exists.", _repositoryPath, data.Id);
                return;
            }
            await WriteToFileAsync("added", data, ct).ConfigureAwait(false);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to add file '{FilePath}/{Id}'!", _repositoryPath, data.Id);
            throw;
        }
    }

    public async Task UpdateAsync(TData data, CancellationToken ct = default) {
        try {
            _logger.LogDebug("Updating data in '{FilePath}/{Id}'...", _repositoryPath, data.Id);
            var filePath = GetActiveFile(data.Id);
            if (filePath is null) {
                _logger.LogDebug("File '{FilePath}/{Id}' not found.", _repositoryPath, data.Id);
                return;
            }

            _io.MoveFile(filePath, filePath.Replace("+", ""));
            await WriteToFileAsync("updated", data, ct).ConfigureAwait(false);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to add or update file '{FilePath}/{Id}'!", _repositoryPath, data.Id);
            throw;
        }
    }

    private async Task WriteToFileAsync(string operation, TData data, CancellationToken ct) {
        var filePath = _io.CombinePath(_repositoryPath, $"+{data.Id}_{_dateTime.Now:yyyyMMddHHmmss}.json");
        await using var stream = _io.CreateNewOrOverwriteFile(filePath);
        await SerializeAsync(stream, data, cancellationToken: ct);
        _logger.LogDebug("File '{FilePath}' {FileOperation}.", filePath, operation);
    }

    public bool Delete(TKey id) {
        try {
            _logger.LogDebug("Deleting file '{FilePath}/{Id}'...", _repositoryPath, id);
            var filePath = GetActiveFile(id);
            if (filePath is null) {
                _logger.LogDebug("File '{FilePath}/{Id}' not found.", _repositoryPath, id);
                return false;
            }

            _io.MoveFile(filePath, filePath.Replace("+", "-"));

            _logger.LogDebug("File '{FilePath}/{Id}' deleted.", _repositoryPath, id);
            return true;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to delete file '{FilePath}/{Id}'!", _repositoryPath, id);
            throw;
        }
    }

    private async Task<TData?> GetFileDataOrDefaultAsync(string filePathWithName, CancellationToken ct) {
        try {
            return await GetFileDataAsync(filePathWithName, ct);
        }
        catch (Exception ex) {
            _logger.LogWarning(ex, "File '{FilePath}' content is invalid.", filePathWithName);
            return default;
        }
    }

    private async Task<TData> GetFileDataAsync(string filePathWithName, CancellationToken ct) {
        await using var stream = _io.OpenFileAsReadOnly(filePathWithName);
        var content = await DeserializeAsync<TData>(stream, cancellationToken: ct);
        _logger.LogDebug("Data from '{FilePath}' retrieved.", filePathWithName);
        return content!;
    }

    private string? GetActiveFile(TKey id) => _io.GetFiles(_repositoryPath, $"+{id}*.json").FirstOrDefault();

    private IEnumerable<string> GetActiveFiles() => _io.GetFiles(_repositoryPath, "+*.json");
}
