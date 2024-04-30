// ReSharper disable once CheckNamespace - Intended to be in this namespace

namespace System.Linq.Expressions;

public record TypeMapper<TSource, TTarget>
    : TypeMapper {
    public TypeMapper(Func<TSource, TTarget>? convert = null)
        : base(typeof(TSource), typeof(TTarget), convert is null ? null : x => convert((TSource)x)!) {
    }
}

public record TypeMapper {
    public TypeMapper(Type sourceType, Type targetType, Func<object, object>? convert = null) {
        SourceType = sourceType;
        TargetType = targetType;
        Convert = convert is null ? null : s => s is null ? null : convert(s);
    }

    public Type SourceType { get; init; }
    public Type TargetType { get; init; }
    public Func<object?, object?>? Convert { get; init; }

    public virtual bool Equals(TypeMapper? other)
        => SourceType == other?.SourceType
        && TargetType == other?.TargetType;
    public override int GetHashCode()
        => HashCode.Combine(SourceType, TargetType);

    public void Deconstruct(out Type sourceType, out Type targetType, out Func<object?, object?> convert) {
        sourceType = SourceType;
        targetType = TargetType;
        convert = Convert;
    }
}
