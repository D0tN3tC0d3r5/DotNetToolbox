﻿namespace DotNetToolbox.Singleton;

public class NamedOptions<TOptions>
    : HasDefault<TOptions>
    , INamedOptions<TOptions>
    where TOptions : NamedOptions<TOptions>, new() {
    private const string _suffix = "Options";
    private static readonly string _typeName = typeof(TOptions).Name;

    // ReSharper disable once StaticMemberInGenericType
    public static string SectionName { get; }
        = _typeName.EndsWith(_suffix)
              ? _typeName.Remove(_typeName.Length - _suffix.Length)
              : _typeName;
}
