// ReSharper disable once CheckNamespace - Intended to be in this namespace

namespace System.Linq.Expressions;

public sealed record TypeMapper<TTarget>
    : TypeMapper {

    public TypeMapper(TTarget constant)
        : this(_ => IsNotNull(constant)) {
    }

    public TypeMapper(Func<TTarget, TTarget>? convert = null)
        : base(typeof(TTarget), typeof(TTarget), convert ?? (s => s)) {
    }

    public TypeMapper(Type sourceType, TTarget constant)
        : this(sourceType, _ => IsNotNull(constant)) {
    }

    public TypeMapper(Type sourceType, Func<object, TTarget> convert)
        : base(sourceType, typeof(TTarget), convert) {
    }
}

public record TypeMapper<TSource, TTarget>
    : TypeMapper {

    public TypeMapper(TTarget constant)
        : this(_ => IsNotNull(constant)) {
    }

    public TypeMapper(Func<TSource, TTarget> convert)
        : base(typeof(TSource), typeof(TTarget), convert) {
    }
}

public record TypeMapper {
    public TypeMapper(Type itemType, object constant)
        : this(itemType, itemType, _ => IsNotNull(constant)) {
    }

    public TypeMapper(Type itemType, Func<object, object>? convert = null)
        : this(itemType, itemType, convert ?? (s => s)) {
    }

    public TypeMapper(Expression expression, Type targetType, object constant)
        : this(expression.Type, targetType, _ => IsNotNull(constant)) {
    }

    public TypeMapper(Expression expression, Type targetType, Func<object, object> convert)
        : this(expression.Type, targetType, convert) {
    }

    public TypeMapper(Type sourceType, Type targetType, object constant)
        : this(sourceType, targetType, _ => IsNotNull(constant)) {
    }

    public TypeMapper(Type sourceType, Type targetType, Func<object, object> convert) {
        SourceType = sourceType;
        TargetType = targetType;
        Convert = s => s is null ? null : convert(s);
    }

    public Type SourceType { get; init; }
    public Type TargetType { get; init; }
    public Func<object?, object?> Convert { get; init; }

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
