﻿namespace DotNetToolbox.Azure;

public interface IAzureSecretReader {
    TValue? GetSecretOrDefault<TValue>(string key, TValue? defaultValue = default);
    string GetSecretOrKey(string key);
}
